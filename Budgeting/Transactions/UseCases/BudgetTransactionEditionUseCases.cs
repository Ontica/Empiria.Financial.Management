/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Use case interactor class               *
*  Type     : BudgetTransactionEditionUseCases           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to edit budget transactions.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.Services;
using Empiria.StateEnums;
using Empiria.Financial;

using Empiria.History.Services;

using Empiria.Budgeting.Adapters;
using Empiria.Budgeting.Transactions.Adapters;

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


    public BudgetTransactionHolderDto CloseTransaction(string budgetTransactionUID) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      transaction.Close();

      transaction.Save();

      HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Cerrada"));

      return BudgetTransactionMapper.Map(transaction);
    }


    public BudgetTransaction CreateTransaction(IPayableEntity payable,
                                               BudgetTransactionFields fields) {
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(payable, nameof(payable));

      var transactionBuilder = new BudgetTransactionBuilder(payable, fields);

      BudgetTransaction transaction = transactionBuilder.Build();

      transaction.SendToAuthorization();

      transaction.Save();

      HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Creada"));

      return transaction;
    }


    public BudgetTransactionHolderDto CreateTransaction(BudgetTransactionFields fields) {
      Assertion.Require(fields, nameof(fields));

      var transactionType = BudgetTransactionType.Parse(fields.TransactionTypeUID);
      var budget = Budget.Parse(fields.BaseBudgetUID);
      var baseEntity = budget; //BaseObject.Parse(fields.BaseEntityTypeUID, fields.BaseEntityUID);

      var transaction = new BudgetTransaction(transactionType, budget, baseEntity);

      transaction.Update(fields);

      transaction.Save();

      HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Creada"));

      return BudgetTransactionMapper.Map(transaction);
    }


    public BudgetEntryDto CreateBudgetEntry(string budgetTransactionUID, BudgetEntryFields fields) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      var entry = transaction.AddEntry(fields);

      transaction.Save();

      return BudgetEntryMapper.Map(entry);
    }


    public BudgetTransactionHolderDto DeleteOrCancelTransaction(string budgetTransactionUID) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      transaction.DeleteOrCancel();

      transaction.Save();

      if (transaction.Status == BudgetTransactionStatus.Canceled) {
        HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Cancelada"));
      } else {
        HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Eliminada"));
      }

      return BudgetTransactionMapper.Map(transaction);
    }


    public BudgetTransactionHolderDto RejectTransaction(string budgetTransactionUID,
                                                        string reason) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      transaction.Reject();

      transaction.Save();

      SetOnReviewAccountsToPending(transaction);

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


    public BudgetAccountDto RequestBudgetAccount(string budgetTransactionUID,
                                                 string requestedSegmentUID) {

      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));
      Assertion.Require(requestedSegmentUID, nameof(requestedSegmentUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);
      var requestedSegment = BudgetAccountSegment.Parse(requestedSegmentUID);
      var orgUnit = (OrganizationalUnit) transaction.BaseParty;

      Assertion.Require(transaction.Rules.CanUpdate,
          "Esta operación sólo está disponible para transacciones abiertas.");

      var searcher = new BudgetAccountSearcher(transaction.BaseBudget.BudgetType);

      Assertion.Require(!searcher.HasSegment(orgUnit, requestedSegment),
          $"{orgUnit.FullName} ya tiene asignada la cuenta presupuestal {requestedSegment.FullName}");


      var newBudgetAccount = new BudgetAccount(BudgetAccountType.GastoCorriente, requestedSegment, orgUnit);

      newBudgetAccount.Save();

      HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Envío de solicitud de autorización de partida presupuestal",
                                                                              $"Partida {newBudgetAccount.Name}"));

      return BudgetAccountMapper.Map(newBudgetAccount);
    }


    public BudgetTransactionHolderDto SendToAuthorization(string budgetTransactionUID) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

      transaction.SendToAuthorization();

      transaction.Save();

      HistoryServices.CreateHistoryEntry(transaction, new HistoryFields("Enviada a autorización"));

      return BudgetTransactionMapper.Map(transaction);
    }


    public BudgetEntryDto UpdateBudgetEntry(string budgetTransactionUID,
                                            string budgetEntryUID,
                                            BudgetEntryFields fields) {
      Assertion.Require(budgetTransactionUID, nameof(budgetTransactionUID));
      Assertion.Require(budgetEntryUID, nameof(budgetEntryUID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureIsValid();

      var transaction = BudgetTransaction.Parse(budgetTransactionUID);

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

    private void SetOnReviewAccountsToPending(BudgetTransaction transaction) {
      var onReviewAccounts = transaction.Entries.FindAll(x => x.BudgetAccount.Status == EntityStatus.OnReview)
                                                .SelectDistinct(x => x.BudgetAccount)
                                                .Sort((x, y) => x.BaseSegment.Code.CompareTo(y.BaseSegment.Code));

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
                                               .Sort((x, y) => x.BaseSegment.Code.CompareTo(y.BaseSegment.Code));

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
