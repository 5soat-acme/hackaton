using HT.Core.Commons.Communication;

namespace HT.Agendas.Application.UseCases.Interfaces;

public interface IRemoverAgendaUseCase
{
    Task<OperationResult> Handle(Guid id);
}