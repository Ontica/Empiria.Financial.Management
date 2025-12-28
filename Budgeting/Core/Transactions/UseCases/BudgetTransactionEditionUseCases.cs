/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Use case interactor class               *
*  Type     : BudgetTransactionEditionUseCases           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to edit budget transactions.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.Financial;
using Empiria.History;
using Empiria.Parties;
using Empiria.Services;
using Empiria.StateEnums;

using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.Data;

namespace Empiria.Budgeting.Transactions.UseCases {

  /// <summary>Use cases used to edit budget transactions.</summary>
  public class BudgetTransactionEditionUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetTransactionEditionUseCases() {
      // no-op
    }

    static public BudgetTransactionEditionUseCases UseCaseInteractor() {
      return CreateInstance<BudgetTransactionEditionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public BudgetTransactionHolderDto AuthorizeTransaction(string budgetTransactionUID) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      transaction.Authorize();

      transaction.Save();

      SetPendingAccountsToOnReview(transaction);

      HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Autorizada"));

      return BudgetTransactionMapper.Map(transaction);
    }


    public int AutoCloseTransactions(Budget budget) {
      var txns = BudgetTransaction.GetFullList<BudgetTransaction>()
                                  .FindAll(x => x.BaseBudget.Equals(budget) && x.Status == TransactionStatus.Authorized);

      int counter = 0;
      foreach (var txn in txns) {
        FixedList<DocumentDto> documents = DocumentServices.GetEntityDocuments(txn);

        if (documents.Count == 2) {
          CloseTransaction(txn.UID);
          counter++;
        }
      }
      return counter;
    }

    public BudgetTransactionHolderDto CloseTransaction(string budgetTransactionUID) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      AssertHasAllDocuments(transaction);

      transaction.Close();

      transaction.Save();

      HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Cerrada"));

      return BudgetTransactionMapper.Map(transaction);
    }


    public BudgetTransactionHolderDto CreateTransaction(BudgetTransactionFields fields) {
      Assertion.Require(fields, nameof(fields));

      var transactionType = BudgetTransactionType.Parse(fields.TransactionTypeUID);

      var budget = Budget.Parse(fields.BaseBudgetUID);

      var party = Party.Parse(fields.BasePartyUID);

      AssertTransactionTypeMultiplicity(transactionType, budget, party);

      BudgetTransaction transaction;

      if (fields.BaseEntityTypeUID.Length != 0) {
        var budgetable = BaseObject.Parse(fields.BaseEntityTypeUID, fields.BaseEntityUID);

        transaction = new BudgetTransaction(transactionType, budget, (IBudgetable) budgetable);

      } else {
        transaction = new BudgetTransaction(transactionType, budget);
      }

      transaction.Update(fields);

      transaction.Save();

      HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Creada"));

      return BudgetTransactionMapper.Map(transaction);
    }


    public BudgetEntryDto CreateBudgetEntry(BudgetEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();

      var transaction = BudgetTransaction.Parse(fields.TransactionUID);

      var entry = transaction.AddEntry(fields);

      transaction.Save();

      return BudgetEntryMapper.Map(entry);
    }


    public BudgetTransactionHolderDto DeleteOrCancelTransaction(string budgetTransactionUID) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      transaction.DeleteOrCancel();

      transaction.Save();

      if (transaction.Status == TransactionStatus.Canceled) {
        HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Cancelada"));
      } else {
        HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Eliminada"));
      }

      return BudgetTransactionMapper.Map(transaction);
    }


    public FixedList<BudgetTransactionDescriptorDto> GeneratePlanningTransactions(string budgetUID) {
      Assertion.Require(budgetUID, nameof(budgetUID));

      var generator = new BudgetTransactionsGenerator();

      Budget budget = Budget.Parse(budgetUID);

      FixedList<BudgetTransaction> transactions = generator.GenerateForPlanning(budget);

      foreach (var transaction in transactions) {
        transaction.Save();

        HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Generada automáticamente"));
      }

      return BudgetTransactionMapper.MapToDescriptor(transactions);
    }


    public BudgetTransactionHolderDto RejectTransaction(string budgetTransactionUID,
                                                        string reason) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      reason = EmpiriaString.Clean(reason);

      transaction.Reject(reason);

      transaction.Save();

      SetOnReviewAccountsToPending(transaction);

      RemoveDocumentsIfNeeded(transaction);

      HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Rechazada", reason));

      return BudgetTransactionMapper.Map(transaction);
    }


    public BudgetEntryDto RemoveBudgetEntry(string budgetTransactionUID, string budgetEntryUID) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));
      Assertion.Require(budgetEntryUID, nameof(budgetEntryUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      var budgetEntry = transaction.GetEntry(budgetEntryUID);

      transaction.RemoveEntry(budgetEntry);

      transaction.Save();

      return BudgetEntryMapper.Map(budgetEntry);
    }


    public BudgetTransactionHolderDto SendToAuthorization(string budgetTransactionUID) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      transaction.SendToAuthorization();

      transaction.Save();

      HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Enviada a autorización"));

      return BudgetTransactionMapper.Map(transaction);
    }


    public BudgetEntryDto UpdateBudgetEntry(string budgetEntryUID,
                                            BudgetEntryFields fields) {
      Assertion.Require(budgetEntryUID, nameof(budgetEntryUID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();

      var transaction = BudgetTransaction.Parse(fields.TransactionUID);

      var budgetEntry = transaction.GetEntry(budgetEntryUID);

      transaction.UpdateEntry(budgetEntry, fields);

      transaction.Save();

      return BudgetEntryMapper.Map(budgetEntry);
    }


    public BudgetTransactionHolderDto UpdateTransaction(string budgetTransactionUID,
                                                        BudgetTransactionFields fields) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));
      Assertion.Require(fields, nameof(fields));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      transaction.Update(fields);

      transaction.Save();

      return BudgetTransactionMapper.Map(transaction);
    }

    #endregion Use cases

    #region Helpers

    private void AssertHasAllDocuments(BudgetTransaction transaction) {
      FixedList<DocumentDto> documents = DocumentServices.GetEntityDocuments(transaction);

      Assertion.Require(documents.Count <= 20, "Para poder cerrar la transacción es necesario subir " +
                                               "todos los documentos que le correspondan.");
    }


    private void AssertTransactionTypeMultiplicity(BudgetTransactionType transactionType, Budget budget, Party baseParty) {

      MultiplicityRule rule = transactionType.MultiplicityRule;

      if (rule == MultiplicityRule.None) {
        return;
      }

      int count = BudgetTransactionDataService.GetTransactions(budget, transactionType)
                                              .FindAll(x => x.BaseParty.Equals(baseParty)).Count;

      if (rule == MultiplicityRule.OnePerYear && count >= 1) {
        Assertion.RequireFail($"Ya existe una transacción del tipo {transactionType.DisplayName} " +
                              $"del presupuesto {budget.Name}, para el área {baseParty.Name}.");
      }

      if (rule == MultiplicityRule.ZeroOrOnePerYear && count >= 1) {
        Assertion.RequireFail($"Ya existe una transacción del tipo {transactionType.DisplayName} " +
                              $"del presupuesto {budget.Name}, para el área {baseParty.Name}.");
      }

    }


    private void RemoveDocumentsIfNeeded(BudgetTransaction transaction) {
      FixedList<DocumentDto> documents = DocumentServices.GetEntityDocuments(transaction);

      foreach (var document in documents) {
        DocumentServices.RemoveDocument(transaction, document);
      }
    }


    private void SetOnReviewAccountsToPending(BudgetTransaction transaction) {
      var onReviewAccounts = transaction.Entries.FindAll(x => x.BudgetAccount.Status == EntityStatus.OnReview)
                                                .SelectDistinct(x => x.BudgetAccount)
                                                .Sort((x, y) => x.Code.CompareTo(y.Code));

      foreach (var account in onReviewAccounts) {
        account.SetStatus(EntityStatus.Pending);

        account.Save();

        HistoryServices.CreateHistoryEntry(transaction,
                                           new HistoryFields("Desautorización de nueva cuenta", account.Name));
      }
    }


    private void SetPendingAccountsToOnReview(BudgetTransaction transaction) {

      var pendingAccounts = transaction.Entries.FindAll(x => x.BudgetAccount.Status == EntityStatus.Pending)
                                               .SelectDistinct(x => x.BudgetAccount)
                                               .Sort((x, y) => x.Code.CompareTo(y.Code));

      foreach (var account in pendingAccounts) {
        account.SetStatus(EntityStatus.OnReview);

        account.Save();

        HistoryServices.CreateHistoryEntry(transaction,
                                           new HistoryFields("Autorización de nueva cuenta", account.Name));
      }
    }

    #endregion Helpers

  }  // class BudgetTransactionEditionUseCases

}  // namespace Empiria.Budgeting.Transactions.UseCases
