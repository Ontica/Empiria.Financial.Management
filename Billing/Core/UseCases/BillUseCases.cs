/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Use cases Layer                         *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Use case interactor class               *
*  Type     : BillUseCases                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage billing.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Financial;
using Empiria.Products;
using Empiria.Services;

using Empiria.Billing.SATMexicoImporter;

using Empiria.Billing.Adapters;
using Empiria.Billing.Data;
using System;

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


    public BillDto CreateBillTest(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      var reader = new SATBillXmlReader(xmlString);
      ISATBillDto satDto = reader.ReadAsBillDto();

      IBillFields fields = BillFieldsMapper.Map((SATBillDto) satDto);
      Bill bill = CreateBillTest((BillFields) fields);

      return BillMapper.MapToBillDto(bill);
    }


    public BillDto CreateBillPaymentComplementTest(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      var reader = new SATPaymentComplementXmlReader(xmlString);
      ISATBillDto satDto = reader.ReadAsPaymentComplementDto();

      IBillFields fields = BillPaymentComplementFieldsMapper.Map((SatBillPaymentComplementDto) satDto);
      Bill bill = CreatePaymentComplementTest((BillPaymentComplementFields) fields);

      return BillMapper.MapToBillDto(bill);
    }


    public BillDto CreateBill(string xmlString, IPayable payable) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));

      var reader = new SATBillXmlReader(xmlString);

      ISATBillDto satDto = reader.ReadAsBillDto();

      IBillFields fields = BillFieldsMapper.Map((SATBillDto) satDto);

      Bill bill = CreateBillImplementation(payable, (BillFields) fields);

      return BillMapper.MapToBillDto(bill);
    }


    public BillDto CreateCreditNote(string xmlString, IPayable payable) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));

      var reader = new SATCreditNoteXmlReader(xmlString);

      ISATBillDto satDto = reader.ReadAsCreditNoteDto();

      IBillFields fields = BillFieldsMapper.Map((SATBillDto) satDto);

      Bill bill = CreateCreditNoteImplementation(payable, (BillFields) fields);

      return BillMapper.MapToBillDto(bill);
    }


    public BillHolderDto GetBill(string billUID) {
      Assertion.Require(billUID, nameof(billUID));

      Bill bill = Bill.Parse(billUID);
      bill.AssignConcepts();

      return BillMapper.Map(bill);
    }


    public BillWithConceptsDto GetBillWithConceptsDto(string billUID) {
      Assertion.Require(billUID, nameof(billUID));

      Bill bill = Bill.Parse(billUID);
      bill.AssignConcepts();

      return BillMapper.MapToBillWithConcepts(bill);
    }


    public FixedList<BillDescriptorDto> SearchBills(BillsQuery query) {
      Assertion.Require(query, nameof(query));

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      FixedList<Bill> bills = BillData.SearchBills(filter, sort);

      return BillMapper.MapToBillListDto(bills);
    }


    public void SetBillAsPayed(string billUID) {

      BillData.SetBillAsPayed(billUID);
    }


    #endregion Use cases

    #region Private methods

    private void AssignConcepts(Bill bill) {

      bill.Concepts = BillConcept.GetListFor(bill);

      foreach (var concept in bill.Concepts) {

        concept.TaxEntries = BillTaxEntry.GetListFor(concept);
      }
    }


    private Bill CreateBillByCategory(IPayable payable, BillFields fields, BillCategory billCategory) {

      var bill = new Bill(payable, billCategory, fields.BillNo);

      bill.Update(fields);

      bill.Save();

      bill.Concepts = CreateBillConcepts(bill, fields.Concepts);

      return bill;
    }


    private FixedList<BillConcept> CreateBillConcepts(Bill bill,
                                                      FixedList<BillConceptWithTaxFields> conceptFields) {
      var concepts = new List<BillConcept>();

      foreach (BillConceptWithTaxFields fields in conceptFields) {

        var billConcept = new BillConcept(bill, Product.Empty);

        billConcept.Update(fields);

        billConcept.Save();

        billConcept.TaxEntries = CreateBillTaxEntries(bill, billConcept, fields.TaxEntries);

        concepts.Add(billConcept);
      }

      return concepts.ToFixedList();
    }


    private FixedList<BillConcept> CreatePaymentComplementConcepts(Bill bill,
                                    FixedList<BillConceptFields> conceptFields) {
      var concepts = new List<BillConcept>();

      foreach (BillConceptFields fields in conceptFields) {

        var billConcept = new BillConcept(bill, Product.Empty);

        billConcept.Update(fields);

        billConcept.Save();

        //billConcept.TaxEntries = CreateBillTaxEntries(bill, billConcept, fields.TaxEntries);

        concepts.Add(billConcept);
      }

      return concepts.ToFixedList();
    }


    private Bill CreateBillImplementation(IPayable payable, BillFields fields) {

      return CreateBillByCategory(payable, fields, BillCategory.FacturaProveedores);
    }


    private FixedList<BillTaxEntry> CreateBillTaxEntries(Bill bill,
                                                         BillConcept billConcept,
                                                         FixedList<BillTaxEntryFields> allTaxesFields) {

      var taxesList = new List<BillTaxEntry>(allTaxesFields.Count);

      foreach (BillTaxEntryFields taxFields in allTaxesFields) {

        var billTaxEntry = new BillTaxEntry(bill, billConcept);

        billTaxEntry.Update(taxFields);

        billTaxEntry.Save();

        taxesList.Add(billTaxEntry);
      }

      return taxesList.ToFixedList();
    }


    private Bill CreateBillTest(BillFields fields) {

      var billCategory = BillCategory.FacturaProveedores;

      var bill = new Bill(billCategory, fields.BillNo);

      bill.Update(fields);

      bill.Save();

      bill.Concepts = CreateBillConcepts(bill, fields.Concepts);

      return bill;
    }


    private Bill CreateCreditNoteImplementation(IPayable payable, BillFields fields) {

      return CreateBillByCategory(payable, fields, BillCategory.NotaDeCreditoProveedores);
    }


    private Bill CreatePaymentComplementImplementation(IPayable payable, BillFields fields) {

      return CreateBillByCategory(payable, fields, BillCategory.FacturaProveedores);
    }


    private Bill CreatePaymentComplementTest(BillPaymentComplementFields fields) {

      var billCategory = BillCategory.ComplementoPagoProveedores;

      var bill = new Bill(billCategory, fields.BillNo);

      bill.UpdatePaymentComplement(fields);

      bill.Save();

      bill.Concepts = CreatePaymentComplementConcepts(bill, fields.Concepts);

      foreach (var complementRelatedPayout in fields.ComplementRelatedPayoutData) {

        bill.BillTaxes = CreatePaymentComplementTaxes(bill, complementRelatedPayout);

      }

      return bill;
    }


    private FixedList<BillTaxEntry> CreatePaymentComplementTaxes(Bill bill,
              ComplementRelatedPayoutDataFields complementRelatedPayoutData) {

      //TODO CREATE COMPLEMENT RELATED PAYOUT DATA

      var taxesList = new List<BillTaxEntry>();
      foreach (var relatedDoc in complementRelatedPayoutData.RelatedDocumentData) {
          //TODO CREATE RELATED DOCUMENT DATA
          
        taxesList.AddRange(CreateBillTaxEntries(bill, new BillConcept(), relatedDoc.Taxes));
      }

      return taxesList.ToFixedList();
    }


    #endregion Private methods

  } // class BillUseCases

} // namespace Empiria.Billing.UseCases
