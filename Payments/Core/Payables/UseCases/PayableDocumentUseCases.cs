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

using Empiria.Documents.Services.Adapters;

using Empiria.Billing;
using Empiria.Documents;

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
                                              DocumentDto documentDto) {
      Assertion.Require(payableUID, nameof(payableUID));
      Assertion.Require(documentDto, nameof(documentDto));

      if (documentDto.ApplicationContentType.Length == 0) {
        return documentDto;
      }

      var payable = Payable.Parse(payableUID);
      var document = Document.Parse(documentDto.UID);

      Bill bill = ExternalServices.GenerateBill(payable, document);

      var linkType = PayableLinkType.Bill;

      var billLink = new PayableLink(linkType, payable, bill);

      billLink.Save();

      SetPayableBillConcepts(payable.GetItems(), bill.Concepts);

      return ExternalServices.UpdatePayableDocumentWithBillData(payable, document, bill);
    }

    private void SetPayableBillConcepts(FixedList<PayableItem> payableItems,
                                        FixedList<BillConcept> billConcepts) {
      for (int i = 0; i < payableItems.Count; i++) {
        PayableItem payableItem = payableItems[i];
        if (i <= billConcepts.Count - 1) {
          payableItem.SetBillConcept(billConcepts[i]);
          payableItem.Save();
        }
      }
    }

    #endregion Use cases

  }  // class PayableDocumentUseCases

}  // namespace Empiria.Payments.Payables.UseCases
