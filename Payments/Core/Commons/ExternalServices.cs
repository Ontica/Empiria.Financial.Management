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

using Empiria.Payments.Payables;
using Empiria.Payments.Orders;

using Empiria.Payments.Processor;
using Empiria.Payments.Processor.Services;

namespace Empiria.Payments {

  /// <summary>Connect Empiria Payments with external services providers.</summary>
  static internal class ExternalServices {

    static internal Bill GenerateBill(Document billDocument) {
      Assertion.Require(billDocument, nameof(billDocument));

      string billXmlFillPath = billDocument.FullLocalName;

      using (var usecases = BillUseCases.UseCaseInteractor()) {

        Billing.Adapters.BillDto returnedValue = usecases.CreateBill(billXmlFillPath);

        return Bill.Parse(returnedValue.UID);
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


    static internal PaymentInstruction SendPaymentOrderToPay(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      using (var usecases = PaymentService.ServiceInteractor()) {

        return usecases.Pay(paymentOrder);
      }
    }


    static internal string GetEntityDocumentsEditionUrl(Payable payable) {
      return $"v2/payments-management/payables/{payable.UID}/documents";
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


    static internal DocumentDto UpdatePayableDocumentWithBillData(Payable payable, Document document, Bill bill) {

      var fields = new DocumentFields {
        UID = document.UID,
        DocumentNo = bill.BillNo,
        DocumentDate = bill.IssueDate
      };

      return DocumentServices.UpdateDocument(payable, document, fields);
    }

  }  // class ExternalServices

}  // namespace Empiria.Payments
