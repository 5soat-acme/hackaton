using HT.Core.Commons.Communication;
using HT.Core.Commons.Repository;

namespace HT.Core.Commons.UseCases;

public abstract class CommonUseCase
{
    protected ValidationResult ValidationResult = new();

    protected void AddError(string message, string propertyName = "")
    {
        ValidationResult.AddError(message, propertyName);
    }

    protected async Task<ValidationResult> PersistData(IUnitOfWork unitOfWork)
    {
        if (!await unitOfWork.Commit()) AddError("Ocorreu um erro ao persistir os dados");
        return ValidationResult;
    }
}