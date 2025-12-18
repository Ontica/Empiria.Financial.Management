/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : External Services Layer                 *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service connector                       *
*  Type     : ExternalServices                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Connect Empiria Payments with external services providers.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.IO;

using Empiria.Documents;
using Empiria.Financial;

using Empiria.Billing;
using Empiria.Billing.Adapters;
using Empiria.Billing.UseCases;

namespace Empiria.Payments {

  /// <summary>Connect Empiria Payments with external services providers.</summary>
  static internal class ExternalServices {

    #region Services

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

    #endregion Services

  }  // class ExternalServices

}  // namespace Empiria.Payments
