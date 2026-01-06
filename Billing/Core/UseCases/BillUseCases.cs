/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Use cases Layer                         *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Use case interactor class               *
*  Type     : BillUseCases                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage billing.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using Empiria.Billing.Adapters;
using Empiria.Billing.Data;
using Empiria.Billing.SATMexicoImporter;
using Empiria.Documents;
using Empiria.Financial;
using Empiria.Services;

namespace Empiria.Billing.UseCases {

  /// <summary>Use cases to manage billing.</summary>
  public class BillUseCases : UseCase {

    #region Constructors and parsers

    protected BillUseCases() {
      // no-op
    }

    static public BillUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<BillUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public Bill CreateCFDI(string xmlString, IPayableEntity payable, DocumentProduct billProduct) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(billProduct, nameof(billProduct));

      string billType = billProduct.ApplicationContentType;

      if (billType == "factura-electronica-sat") {
        return CreateBill(xmlString, payable);

      } else if (billType == "nota-credito-sat") {
        return CreateCreditNote(xmlString, payable);

      } else {
        throw Assertion.EnsureNoReachThisCode($"Unrecognized applicationContentType '{billType}'.");
      }
    }


    public Bill CreateFuelConsumptionBill(string xmlString, IPayableEntity payable) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));

      var reader = new SATFuelConsumptionBillXmlReader(xmlString);

      ISATBillDto satDto = reader.ReadAsFuelConsumptionBillDto();

      IBillFields fields = FuelConsumptionBillMapper.Map((SATFuelConsumptionBillDto) satDto);

      return CreateFuelConsumptionImplementation(payable, (FuelConsumptionBillFields) fields);
    }


    public Bill CreateVoucherBill(IPayableEntity payable, DocumentFields fields) {
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(fields, nameof(fields));

      Assertion.Require(fields.DocumentNumber, "Requiero el número de oficio o documento que ampara al comprobante.");
      Assertion.Require(fields.Name, "Requiero la descripción del oficio o documento que ampara al comprobante.");
      Assertion.Require(fields.Total, "Requiero el total del comprobante");

      var bill = new Bill(payable, BillCategory.Parse(97), fields);

      bill.Save();

      return bill;
    }


    public string ExtractCFDINo(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      var reader = new SATBillXmlReader(xmlString);

      SATBillDto satDto = reader.ReadAsBillDto();

      return satDto.SATComplemento.UUID;
    }


    private Bill CreateBill(string xmlString, IPayableEntity payable) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));

      var reader = new SATBillXmlReader(xmlString);

      ISATBillDto satDto = reader.ReadAsBillDto();

      IBillFields fields = BillFieldsMapper.Map((SATBillDto) satDto);

      return CreateBillImplementation(payable, (BillFields) fields);
    }


    private BillDto CreateBillPaymentComplement(string xmlString, IPayableEntity payable) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));

      var reader = new SATPaymentComplementXmlReader(xmlString);

      ISATBillDto satDto = reader.ReadAsPaymentComplementDto();

      IBillFields fields = BillPaymentComplementFieldsMapper.Map((SatBillPaymentComplementDto) satDto);

      Bill bill = CreatePaymentComplementImplementation(payable, (BillPaymentComplementFields) fields);

      return BillMapper.MapToBillDto(bill);
    }


    private Bill CreateCreditNote(string xmlString, IPayableEntity payable) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));

      var reader = new SATCreditNoteXmlReader(xmlString);

      ISATBillDto satDto = reader.ReadAsCreditNoteDto();

      IBillFields fields = BillFieldsMapper.Map((SATBillDto) satDto);

      return CreateCreditNoteImplementation(payable, (BillFields) fields);
    }


    public BillHolderDto GetBill(string billUID) {
      Assertion.Require(billUID, nameof(billUID));

      Bill bill = Bill.Parse(billUID);

      return BillMapper.Map(bill);
    }


    internal BillWithConceptsDto GetBillWithConceptsDtoTests(string billUID) {
      Assertion.Require(billUID, nameof(billUID));

      Bill bill = Bill.Parse(billUID);

      return BillMapper.MapToBillWithConcepts(bill);
    }


    public FixedList<BillDescriptorDto> SearchBills(BillsQuery query) {
      Assertion.Require(query, nameof(query));

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      FixedList<Bill> bills = BillData.SearchBills(filter, sort);

      return BillMapper.MapToBillListDto(bills);
    }

    #endregion Use cases

    #region Private methods

    private Bill CreateBillByCategory(IPayableEntity payable, BillFields fields, BillCategory billCategory) {

      var bill = new Bill(payable, billCategory, fields.BillNo);

      bill.Update(fields);

      bill.Save();

      foreach (var fieldsConcept in fields.Concepts) {
        bill.AddConcept(BillConceptType.Default, fieldsConcept);
      }

      foreach (var addendaConcept in fields.Addenda.Concepts) {
        bill.AddConcept(BillConceptType.Addenda, addendaConcept);
      }

      return bill;
    }


    private Bill CreateBillImplementation(IPayableEntity payable, BillFields fields) {

      return CreateBillByCategory(payable, fields, BillCategory.FacturaProveedores);
    }


    private Bill CreateFuelConsumptionImplementation(IPayableEntity payable, FuelConsumptionBillFields fields) {

      var billCategory = BillCategory.FacturaConsumoCombustible;

      var bill = new Bill(payable, billCategory, fields.BillNo);

      bill.UpdateFuelConsumptionBill(fields);

      bill.Save();

      foreach (var fieldsConcept in fields.Concepts) {
        bill.AddConcept(BillConceptType.Default, fieldsConcept);
      }

      foreach (var fieldsConcept in fields.ComplementData.ComplementConcepts) {
        bill.AddComplementConcepts(fieldsConcept);
      }

      foreach (var fieldsConcept in fields.Addenda.Concepts) {
        bill.AddConcept(BillConceptType.Addenda, fieldsConcept);
      }

      return bill;
    }


    private void CreateBillTaxEntries(Bill bill, int billRelatedDocumentId,
                                      FixedList<BillTaxEntryFields> allTaxesFields) {

      foreach (BillTaxEntryFields taxFields in allTaxesFields) {

        var billTaxEntry = new BillTaxEntry(bill, billRelatedDocumentId, taxFields);

        billTaxEntry.Save();
      }
    }


    private void CreateBillRelatedBills(Bill bill,
                                        FixedList<ComplementRelatedPayoutDataFields> complementRelatedPayoutData) {

      foreach (var relatedPayoutFields in complementRelatedPayoutData) {

        var billRelated = new BillRelatedBill(bill);

        billRelated.Update(relatedPayoutFields);
        billRelated.Save();

        CreateBillTaxEntries(bill, billRelated.Id, relatedPayoutFields.Taxes);
      }
    }


    private Bill CreateCreditNoteImplementation(IPayableEntity payable, BillFields fields) {

      return CreateBillByCategory(payable, fields, BillCategory.NotaDeCreditoProveedores);
    }


    private void CreatePaymentComplementConcepts(Bill bill, FixedList<BillConceptFields> conceptFields) {

      foreach (BillConceptFields fields in conceptFields) {

        var billConcept = new BillConcept(bill, fields);

        billConcept.Save();
      }
    }


    private Bill CreatePaymentComplementImplementation(IPayableEntity payable, BillPaymentComplementFields fields) {

      var billCategory = BillCategory.ComplementoPagoProveedores;

      var bill = new Bill(payable, billCategory, fields.BillNo);

      bill.UpdatePaymentComplement(fields);

      bill.Save();

      CreatePaymentComplementConcepts(bill, fields.Concepts);

      CreateBillRelatedBills(bill, fields.ComplementRelatedPayoutData);

      return bill;
    }

    #endregion Private methods

  } // class BillUseCases

} // namespace Empiria.Billing.UseCases
