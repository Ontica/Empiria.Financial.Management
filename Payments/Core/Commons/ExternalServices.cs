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

using Empiria.Parties;

using Empiria.Documents;
using Empiria.Documents.Services;
using Empiria.Documents.Services.Adapters;

using Empiria.Billing;
using Empiria.Billing.Adapters;
using Empiria.Billing.UseCases;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.UseCases;

using Empiria.Procurement.Contracts;

using Empiria.Payments.Payables;
using Empiria.Payments.Orders;

using Empiria.Payments.Processor;
using Empiria.Payments.Processor.Services;
using System.Threading.Tasks;
using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;


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


    static internal Bill GenerateBill(Payable payable, Document billDocument) {
      Assertion.Require(billDocument, nameof(billDocument));

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        BillDto returnedValue;

        string billType = billDocument.DocumentProduct.ApplicationContentType;

        if (billType == "factura-electronica-sat") {
          returnedValue = usecases.CreateBill(billDocument.ReadAllText(), payable);
        } else if (billType == "nota-credito-sat") {
          returnedValue = usecases.CreateCreditNote(billDocument.ReadAllText(), payable);
        } else {
          throw Assertion.EnsureNoReachThisCode($"Unrecognized applicationContentType '{billType}'.");
        }

        return Bill.Parse(returnedValue.UID);
      }
    }


    static internal FixedList<BillDto> GetPayableBills(Payable payable) {
      Assertion.Require(payable, nameof(payable));

      return new FixedList<BillDto>();
    }


    static internal PaymentInstruction SendPaymentOrderToPay(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      using (var usecases = PaymentService.ServiceInteractor()) {

        return usecases.Pay(paymentOrder);
      }
    }


    static internal DocumentDto UpdatePayableDocumentWithBillData(Payable payable, Document document, Bill bill) {

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


    static internal PaymentInstruction ValidateIsPaymentInstructionPayed(string paymentInstructionUD) {
      using (var usecases = PaymentService.ServiceInteractor()) {

        return usecases.ValidateIsPaymentInstructionPayed(paymentInstructionUD);
      }
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
        BudgetTransaction transaction = usecases.CreateTransaction(payable.PayableEntity, fields);

        CreateBudgetTransactionDocumentLinks(payable, transaction);
      }
    }


    static private BudgetTransactionFields TransformToBudgetTransactionFields(Payable payable,
                                                                              BudgetTransactionType transactionType,
                                                                              DateTime applicationDate) {

      var SISTEMA_DE_PAGOS = OperationSource.Parse(12);

      int contractId = -1;

      if (payable.PayableEntity is Contract contract) {
        contractId = contract.Id;
      }
      if (payable.PayableEntity is ContractOrder contractOrder) {
        contractId = contractOrder.Contract.Id;
      }

      return new BudgetTransactionFields {
        TransactionTypeUID = transactionType.UID,
        BaseBudgetUID = payable.Budget.UID,
        OperationSourceUID = SISTEMA_DE_PAGOS.UID,
        Description = payable.Description,
        PayableId = payable.Id,
        BasePartyUID = payable.OrganizationalUnit.UID,
        RequestedByUID = payable.PostedBy.UID,
        ApplicationDate = applicationDate
      };
    }


    static private void CreatePayableDocumentLinks(Payable payable, Document document, Bill bill) {
      DocumentLinkServices.CreateLink(document, bill);

      DocumentLinkServices.CreateLink(document, payable.GetPayableEntity());

      foreach (var payableEntityDocument in DocumentServices.GetEntityBaseDocuments(payable.GetPayableEntity())) {
        DocumentLinkServices.CreateLink(Document.Parse(payableEntityDocument.UID), bill);
      }

      // if (payable.GetPayableEntity() is ContractOrder contractOrder) {

      //   foreach (var contractDocument in DocumentServices.GetEntityBaseDocuments(contractOrder.Contract)) {
      //     DocumentLinkServices.CreateLink(Document.Parse(contractDocument.UID), bill);
      //   }
      //}
    }


    static private void CreateBudgetTransactionDocumentLinks(Payable payable, BudgetTransaction transaction) {

      foreach (var payableDocument in DocumentServices.GetEntityBaseDocuments(payable)) {
        DocumentLinkServices.CreateLink(Document.Parse(payableDocument.UID), transaction);
      }

      foreach (var payableEntityDocument in DocumentServices.GetEntityBaseDocuments(payable.GetPayableEntity())) {
        DocumentLinkServices.CreateLink(Document.Parse(payableEntityDocument.UID), transaction);
      }


      // if (payable.GetPayableEntity() is ContractOrder contractOrder) {

      // foreach (var contractDocument in DocumentServices.GetEntityBaseDocuments(contractOrder.Contract)) {
      //   DocumentLinkServices.CreateLink(Document.Parse(contractDocument.UID), transaction);
      // }
      // }
    }

    #endregion Helpers

  }  // class ExternalServices

}  // namespace Empiria.Payments
