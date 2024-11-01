/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : PayableBillUseCases                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for payable bills management.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Services;
using Empiria.Storage;

using Empiria.Documents;
using Empiria.Documents.Services.Adapters;

using Empiria.Billing;

using Empiria.Payments.Payables.Adapters;

namespace Empiria.Payments.Payables.UseCases {

  /// <summary>Use cases for payable bills management.</summary>
  public class PayableBillUseCases : UseCase {

    #region Constructors and parsers

    protected PayableBillUseCases() {
      // no-op
    }

    static public PayableBillUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<PayableBillUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public PayableHolderDto AppendPayableBill(string payableUID,
                                              DocumentFields fields,
                                              InputFile billFile) {
      Assertion.Require(payableUID, nameof(payableUID));
      Assertion.Require(fields, nameof(fields));
      Assertion.Require(billFile, nameof(billFile));

      // ToDo Validar bill con SATXmlReader / Validator

      var payable = Payable.Parse(payableUID);

      DocumentDto billAsDocument = ExternalServices.StorePayableBillAsDocument(payable, billFile, fields);

      Bill bill = ExternalServices.GenerateBill(billAsDocument);

      var linkType = PayableLinkType.Bill;

      var billLink = new PayableLink(linkType, payable, bill);

      billLink.Save();

      return PayableHolderMapper.Map(payable);
    }


    public void DeletePayableBill(string payableUID, string billUID) {
      Assertion.Require(payableUID, nameof(payableUID));
      Assertion.Require(billUID, nameof(billUID));

      throw new NotImplementedException();
    }

    #endregion Use cases

  }  // class PayableBillUseCases

}  // namespace Empiria.Payments.Payables.UseCases
