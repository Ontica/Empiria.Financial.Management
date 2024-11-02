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
using System.Collections.Generic;

using Empiria.Storage;

using Empiria.Documents;
using Empiria.Documents.Services;
using Empiria.Documents.Services.Adapters;

using Empiria.History.Services;
using Empiria.History.Services.Adapters;

using Empiria.Billing;
using Empiria.Billing.UseCases;
using Empiria.Billing.Adapters;

using Empiria.Payments.Payables;

namespace Empiria.Payments {

  /// <summary>Connect Empiria Payments with external services providers.</summary>
  static internal class ExternalServices {

    static internal Bill GenerateBill(DocumentDto billDocument) {
      Assertion.Require(billDocument, nameof(billDocument));

      var document = Document.Parse(billDocument.UID);

      string billXmlFillPath = document.FullLocalName;

      using (var usecases = BillUseCases.UseCaseInteractor()) {

                Billing.Adapters.BillDto returnedValue = usecases.CreateBill(billXmlFillPath);

        return Bill.Parse(returnedValue.UID);
      }
    }


    static internal FixedList<DocumentDto> GetEntityDocuments(BaseObject entity) {
      Assertion.Require(entity, nameof(entity));

      using (var services = DocumentServices.ServiceInteractor()) {

        return services.GetEntityDocuments(entity);
      }
    }


    static internal FixedList<HistoryDto> GetEntityHistory(BaseObject entity) {
      Assertion.Require(entity, nameof(entity));

      using (var services = HistoryServices.ServiceInteractor()) {

        return services.GetEntityHistory(entity);
      }
    }


    static internal FixedList<BillDto> GetPayableBills(Payable payable) {
      var bill = new BillDto {
        UID = "e0f6a221-bf23-429a-a503-bfd63eb581fa",
        Name = "LA VÍA ONTICA SC",
        Date = DateTime.Today,
        CFDIGUID = "-bf23-429a-a503-bfd63eb581",
        Items = MapBillItems()
      };

      List<BillDto> bills = new List<BillDto>();

      bills.Add(bill);

      return bills.ToFixedList();
    }

    static internal DocumentDto StorePayableBillAsDocument(Payable payable,
                                                           InputFile billFile,
                                                           DocumentFields fields) {
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(billFile, nameof(billFile));
      Assertion.Require(fields, nameof(fields));

      using (var services = DocumentServices.ServiceInteractor()) {

        return services.CreateDocument(billFile, payable, fields);
      }
    }


    static private FixedList<BillItemDto> MapBillItems() {
      var item = new BillItemDto {
        UID = "aa4ca009-a204-4732-8f5c-c1bf3a25019f",
        Name = "345 Puntos de función COSMIC",
        Unit = "Puntos de función CFP2",
        Quantity = 1,
        Total = 4004.898m
      };

      List<BillItemDto> items = new List<BillItemDto>();
      items.Add(item);

      return items.ToFixedList();
    }

  }  // class ExternalServices

}  // namespace Empiria.Payments
