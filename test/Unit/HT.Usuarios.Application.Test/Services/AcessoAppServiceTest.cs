
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FluentAssertions.Common;
using HT.Core.Commons.Identity;
using HT.Usuarios.Application.DTOs.Requests;
using HT.Usuarios.Application.Services;
using HT.Usuarios.Infra.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace HT.Usuarios.Application.Test.Services;

public class AcessoAppServiceTest
{
    /*private readonly IFixture _fixture;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private readonly Mock<IOptions<IdentitySettings>> _settingsMock;
    private readonly AcessoAppService _acessoAppService;*/

    private readonly IFixture _fixture;
    private readonly Mock<IUserStore<ApplicationUser>> _userStoreMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly AcessoAppService _acessoAppService;

    public AcessoAppServiceTest()
    {
        /*_fixture = new Fixture().Customize(new AutoMoqCustomization());
        _userManagerMock = _fixture.Freeze<Mock<UserManager<ApplicationUser>>>();
        _roleManagerMock = _fixture.Freeze<Mock<RoleManager<IdentityRole>>>();
        _signInManagerMock = _fixture.Freeze<Mock<SignInManager<ApplicationUser>>>();
        _settingsMock = _fixture.Freeze<Mock<IOptions<IdentitySettings>>>();
        _acessoAppService = _fixture.Create<AcessoAppService>();*/

        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        // Configurando o mock do IUserStore<ApplicationUser>
        _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            _userStoreMock.Object,
            null, null, null, null, null, null, null, null);
        _signInManagerMock = _fixture.Freeze<Mock<SignInManager<ApplicationUser>>>();
        _roleManagerMock = _fixture.Freeze<Mock<RoleManager<IdentityRole>>>();

        var identitySettings = _fixture.Create<IdentitySettings>();
        _acessoAppService = new AcessoAppService(
            _userManagerMock.Object,
            _roleManagerMock.Object,
            _signInManagerMock.Object,
            Options.Create(identitySettings)
        );
    }

    [Fact]
    public async Task DeveCriarUsuario()
    {
        // Arrange
        var novoUsuario = _fixture.Create<NovoUsuario>();

        _roleManagerMock.Setup(m => m.RoleExistsAsync(novoUsuario.TipoAcesso.ToString()))
                        .ReturnsAsync(false);
        _roleManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityRole>()))
                        .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), novoUsuario.Senha))
                        .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), novoUsuario.TipoAcesso.ToString()))
                        .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _acessoAppService.CriarUsuario(novoUsuario);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task DeveGerarTokenDeAcesso()
    {
        // Arrange
        var usuario = _fixture.Create<UsuarioAcesso>();
        var applicationUser = _fixture.Create<ApplicationUser>();
        var claims = new List<Claim>();
        var roles = new List<string>
        {
            TipoAcesso.MEDICO.ToString()
        };

        _signInManagerMock.Setup(m => m.PasswordSignInAsync(usuario.Email, usuario.Senha, false, false))
                          .ReturnsAsync(SignInResult.Success);
        _userManagerMock.Setup(m => m.FindByEmailAsync(usuario.Email))
                        .ReturnsAsync(applicationUser);
        _userManagerMock.Setup(m => m.GetClaimsAsync(applicationUser))
                        .ReturnsAsync(claims);
        _userManagerMock.Setup(m => m.GetRolesAsync(applicationUser))
                        .ReturnsAsync(roles);

        // Act
        var result = await _acessoAppService.Identificar(usuario);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task DeveRetornarErro_QuandoAcessarComUsuarioIncorreto()
    {
        // Arrange
        var usuario = _fixture.Create<UsuarioAcesso>();

        _signInManagerMock.Setup(m => m.PasswordSignInAsync(usuario.Email, usuario.Senha, false, false))
                          .ReturnsAsync(SignInResult.Failed);
        _userManagerMock.Setup(m => m.FindByEmailAsync(usuario.Email))
                        .ReturnsAsync((ApplicationUser?)null);

        // Act
        var result = await _acessoAppService.Identificar(usuario);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Data.Should().BeNull();
        result.GetErrorMessages().Count(x => x == "Usuário inválido").Should().Be(1);
    }

    [Fact]
    public async Task DeveRetornarErro_QuandoNaoEncontrarUsuarioPorEmail()
    {
        // Arrange
        var usuario = _fixture.Create<UsuarioAcesso>();

        // Configurar o mock para retornar sucesso na autenticação
        _signInManagerMock.Setup(m => m.PasswordSignInAsync(usuario.Email, usuario.Senha, false, false))
                          .ReturnsAsync(SignInResult.Success);

        // Configurar o mock para retornar null quando o usuário é buscado
        _userManagerMock.Setup(m => m.FindByEmailAsync(usuario.Email))
                        .ReturnsAsync((ApplicationUser)null);

        // Act
        var result = await _acessoAppService.Identificar(usuario);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Data.Should().BeNull();
        result.GetErrorMessages().Count(x => x == "Usuário inválido").Should().Be(1);
    }
}
