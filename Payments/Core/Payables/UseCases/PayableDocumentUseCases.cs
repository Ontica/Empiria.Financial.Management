/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : PayableDocumentUseCases                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for payable documents management.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Services;

using Empiria.Documents;

using Empiria.Billing;

namespace Empiria.Payments.Payables.UseCases {

  /// <summary>Use cases for payable bills management.</summary>
  public class PayableDocumentUseCases : UseCase {

    #region Constructors and parsers

    protected PayableDocumentUseCases() {
      // no-op
    }

    static public PayableDocumentUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<PayableDocumentUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public DocumentDto ProcessPayableDocument(string payableUID,
                                              DocumentDto document) {
      Assertion.Require(payableUID, nameof(payableUID));
      Assertion.Require(document, nameof(document));

      if (document.ApplicationContentType.Length == 0) {
        return document;
      }

      throw new NotImplementedException();

      //var payable = Payable.Parse(payableUID);

      //Bill bill = ExternalServices.GenerateBill(payable, document);

      //SetPayableBillConcepts(payable.GetItems(), bill.Concepts);

      //return ExternalServices.UpdatePayableDocumentWithBillData(payable, document, bill);
    }


    private void SetPayableBillConcepts(FixedList<PayableItem> payableItems,
                                        FixedList<BillConcept> billConcepts) {
      for (int i = 0; i < payableItems.Count; i++) {
        PayableItem payableItem = payableItems[i];
        if (i <= billConcepts.Count - 1) {

          payableItem.Save();
        }
      }
    }

    #endregion Use cases

  }  // class PayableDocumentUseCases

}  // namespace Empiria.Payments.Payables.UseCases
