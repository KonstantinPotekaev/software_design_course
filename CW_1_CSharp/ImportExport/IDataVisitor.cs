using CW_1_CSharp.Domain;

namespace CW_1_CSharp.ImportExport
{
    public interface IDataVisitor
    {
        void VisitBankAccount(BankAccount bankAccount);
        void VisitCategory(Category category);
        void VisitOperation(Operation operation);

        string GetResult();
    }
}