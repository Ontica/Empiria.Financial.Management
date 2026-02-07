/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : PaymentsConceptsReportBuilder                 License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a dynamic report with all payments in a time period with their concepts.                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.DynamicData;
using Empiria.Financial;

using Empiria.Budgeting;

namespace Empiria.Payments.Reporting {

  public class PaymentConceptDto {

    public string PaymentOrderUID {
      get; internal set;
    }

    public string PaymentOrderNo {
      get; internal set;
    }

    public string PayToName {
      get; internal set;
    }

    public string PayToCode {
      get; internal set;
    }

    public DateTime PaymentDate {
      get; internal set;
    }

    public string PaymentMethod {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public string OrgUnitCode {
      get; internal set;
    }

    public string OrgUnitName {
      get; internal set;
    }

    public string ConceptCode {
      get; internal set;
    }

    public string ConceptName {
      get; internal set;
    }

    public string ConceptDescription {
      get; internal set;
    }

    public decimal ConceptAmount {
      get; internal set;
    }

  }  // class PaymentConceptDto



  /// <summary>Builds a dynamic report with all payments in a time period with their concepts.</summary>
  internal class PaymentsConceptsReportBuilder {

    private readonly DateTime _fromDate;
    private readonly DateTime _toDate;

    public PaymentsConceptsReportBuilder(DateTime fromDate, DateTime toDate) {
      _fromDate = fromDate;
      _toDate = toDate;
    }


    internal FixedList<DataTableColumn> BuildColumns() {
      return new List<DataTableColumn>() {
        new DataTableColumn("paymentOrderNo", "Solicitud de pago", "text-link", linkField: "paymentOrderUID"),
        new DataTableColumn("payToName", "Beneficiario", "text"),
        new DataTableColumn("payToCode", "RFC", "text"),
        new DataTableColumn("paymentDate", "Fecha de pago", "date"),
        new DataTableColumn("paymentMethod", "Forma de pago", "text"),
        new DataTableColumn("total", "Total pagado", "decimal"),
        new DataTableColumn("orgUnitCode", "Area", "text"),
        new DataTableColumn("orgUnitName", "Nombre área", "text"),
        new DataTableColumn("conceptCode", "Partida", "text"),
        new DataTableColumn("conceptName", "Nombre partida", "text"),
        new DataTableColumn("conceptDescription", "Descripción", "text"),
        new DataTableColumn("conceptAmount", "Importe partida", "decimal"),
      }.ToFixedList();
    }


    internal FixedList<PaymentConceptDto> BuildEntries() {
      FixedList<PaymentOrder> paymentOrders = GetPaymentOrders();

      return paymentOrders.SelectFlat(x => CreatePaymentConceptsDto(x));
    }

    #region Helpers

    private FixedList<PaymentConceptDto> CreatePaymentConceptsDto(PaymentOrder paymentOrder) {

      var concepts = paymentOrder.PayableEntity.Items;

      return concepts.Select(concept => CreatePaymentConceptDto(paymentOrder, concept))
                     .ToFixedList();
    }


    private PaymentConceptDto CreatePaymentConceptDto(PaymentOrder paymentOrder, IPayableEntityItem concept) {
      BudgetAccount account = (BudgetAccount) concept.BudgetAccount;

      return new PaymentConceptDto {
        PaymentOrderUID = paymentOrder.UID,
        PaymentOrderNo = paymentOrder.PaymentOrderNo,
        PayToName = paymentOrder.PayTo.Name,
        PayToCode = paymentOrder.PayTo.Code,
        PaymentDate = paymentOrder.PostingTime,
        PaymentMethod = paymentOrder.PaymentMethod.Name,
        Total = paymentOrder.Total,
        ConceptCode = account.AccountNo,
        ConceptName = account.StandardAccount.Name,
        ConceptDescription = concept.Description,
        OrgUnitCode = account.OrganizationalUnit.Code,
        OrgUnitName = account.OrganizationalUnit.Name,
        ConceptAmount = concept.Subtotal
      };
    }


    private FixedList<PaymentOrder> GetPaymentOrders() {
      return PaymentOrder.GetList<PaymentOrder>()
                         .FindAll(x => x.Status == PaymentOrderStatus.Payed &&
                                                   _fromDate <= x.PostingTime && x.PostingTime < _toDate.AddDays(1))
                         .ToFixedList()
                         .Sort((x, y) => x.PostingTime.CompareTo(y.PostingTime));
    }

    #endregion Helpers

  }  // class PaymentsConceptsReportBuilder

}  // namespace Empiria.Payments.Reporting
