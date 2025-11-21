/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : External Services Layer                 *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service connector                       *
*  Type     : ExternalServices                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Connect Empiria Payments with external services providers.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.IO;

using Empiria.Documents;
using Empiria.Financial;
using Empiria.Parties;

using Empiria.Billing;
using Empiria.Billing.Adapters;
using Empiria.Billing.UseCases;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.UseCases;

using Empiria.Payments.Payables;

namespace Empiria.Payments {

  /// <summary>Connect Empiria Payments with external services providers.</summary>
  static internal class ExternalServices {

    #region Services

    static internal void CommitBudget(Payable payable) {
      InvokeBudgetTransactionService(payable, BudgetTransactionType.ComprometerGastoCorriente, payable.DueTime);
    }


    static internal void ExerciseBudget(Payable payable) {
      InvokeBudgetTransactionService(payable, BudgetTransactionType.EjercerGastoCorriente, DateTime.Today);
    }


    static internal Bill GenerateBill(IPayableEntity payable, DocumentDto billDocument) {
      Assertion.Require(billDocument, nameof(billDocument));

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        BillDto returnedValue;

        string billType = billDocument.ApplicationContentType;

        if (billType == "factura-electronica-sat") {
          returnedValue = usecases.CreateBill(File.ReadAllText(billDocument.FullLocalName), payable);
        } else if (billType == "nota-credito-sat") {
          returnedValue = usecases.CreateCreditNote(File.ReadAllText(billDocument.FullLocalName), payable);
        } else {
          throw Assertion.EnsureNoReachThisCode($"Unrecognized applicationContentType '{billType}'.");
        }

        return Bill.Parse(returnedValue.UID);
      }
    }


    static internal FixedList<BillDto> GetPayableBills(IPayableEntity payable) {
      Assertion.Require(payable, nameof(payable));

      return new FixedList<BillDto>();
    }


    static internal DocumentDto UpdatePayableDocumentWithBillData(Payable payable, DocumentDto document, Bill bill) {

      CreatePayableDocumentLinks(payable, document, bill);

      var fields = new DocumentFields {
        UID = document.UID,
        DocumentNo = bill.BillNo,
        DocumentDate = bill.IssueDate,
        SourcePartyUID = bill.IssuedBy.UID,
        TargetPartyUID = bill.IssuedTo.UID,
        Description = payable.Description
      };

      return DocumentServices.UpdateDocument(payable, document, fields);
    }

    #endregion Services

    #region Helpers

    static private void InvokeBudgetTransactionService(Payable payable,
                                                       BudgetTransactionType budgetTransactionType,
                                                       DateTime applicationDate) {

      BudgetTransactionFields fields = TransformToBudgetTransactionFields(payable,
                                                                          budgetTransactionType,
                                                                          applicationDate);

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {
        var transaction = usecases.CreateTransaction(fields);

        CreateBudgetTransactionDocumentLinks(payable, BudgetTransaction.Parse(transaction.Transaction.UID));
      }
    }


    static private BudgetTransactionFields TransformToBudgetTransactionFields(Payable payable,
                                                                              BudgetTransactionType transactionType,
                                                                              DateTime applicationDate) {
      return new BudgetTransactionFields {
        TransactionTypeUID = transactionType.UID,
        BaseBudgetUID = Budget.Parse(1).UID,
        OperationSourceUID = OperationSource.ParseNamedKey("SISTEMA_DE_PAGOS").UID,
        Description = payable.Description,
        PayableId = payable.Id,
        BasePartyUID = payable.OrganizationalUnit.UID,
        RequestedByUID = payable.PostedBy.UID,
        ApplicationDate = applicationDate
      };
    }


    static private void CreatePayableDocumentLinks(Payable payable, DocumentDto document, Bill bill) {
      DocumentLinkServices.CreateLink(document, bill);

      DocumentLinkServices.CreateLink(document, payable.GetPayableEntity());

      foreach (var baseDocument in DocumentServices.GetEntityDocuments(payable.GetPayableEntity())) {
        DocumentLinkServices.CreateLink(baseDocument, bill);
      }

    }


    static private void CreateBudgetTransactionDocumentLinks(Payable payable, BudgetTransaction transaction) {

      foreach (var baseDocument in DocumentServices.GetEntityDocuments(payable)) {
        DocumentLinkServices.CreateLink(baseDocument, transaction);
      }

      foreach (var baseDocument in DocumentServices.GetEntityDocuments(payable.GetPayableEntity())) {
        DocumentLinkServices.CreateLink(baseDocument, transaction);
      }
    }

    #endregion Helpers

  }  // class ExternalServices

}  // namespace Empiria.Payments
