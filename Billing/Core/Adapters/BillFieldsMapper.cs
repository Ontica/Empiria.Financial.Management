/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Mapping class                           *
*  Type     : BillFieldsMapper                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for bill fields.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using Empiria.Billing.SATMexicoImporter;

namespace Empiria.Billing.Adapters {

  /// <summary>Mapping methods for bill fields.</summary>
  static internal class BillFieldsMapper {


    #region Public methods

    static internal BillFields Map(SATBillDto satTBillDto) {

      return MapToBillFields(satTBillDto);
    }

    #endregion Public methods

    #region Private methods

    static private FixedList<BillConceptFields> MapToBillConceptFields(FixedList<SATBillConceptDto> conceptos) {

      List<BillConceptFields> fields = new List<BillConceptFields>();

      foreach (var concepto in conceptos) {

        var field = new BillConceptFields();
        field.ProductUID = string.Empty;
        field.Description = concepto.Descripcion;
        field.Quantity = concepto.Cantidad;
        field.UnitPrice = concepto.ValorUnitario;
        field.Subtotal = concepto.Importe;

        field.TaxEntries = MapToBillTaxFields(concepto.Impuestos);

        field.BillUID = string.Empty;
        field.Identificators = string.Empty;
        field.Tags = string.Empty;
        field.Discount = 0;
        field.SchemaExtData = string.Empty;
        field.ExtData = string.Empty;

        fields.Add(field);
      }

      return fields.ToFixedList();
    }


    static private BillFields MapToBillFields(SATBillDto dto) {

      BillFields fields = new BillFields();

      fields.BillNo = dto.DatosGenerales.NoCertificado;
      fields.IssueDate = dto.DatosGenerales.Fecha;
      fields.IssuedByRFC = dto.Emisor.RFC;
      fields.IssuedToRFC = dto.Receptor.RFC;
      fields.SchemaVersion = dto.DatosGenerales.CFDIVersion;
      fields.CurrencyCode = dto.DatosGenerales.Moneda;
      fields.Subtotal = dto.DatosGenerales.SubTotal;
      fields.Total = dto.DatosGenerales.Total;
      fields.Concepts = MapToBillConceptFields(dto.Conceptos);

      fields.ManagedByUID = string.Empty;
      fields.BillCategoryUID = string.Empty;
      fields.Identificators = string.Empty;
      fields.Tags = string.Empty;
      fields.Discount = 0;
      fields.SchemaExtData = string.Empty;
      fields.SecurityExtData = string.Empty;
      fields.PaymentExtData = string.Empty;
      fields.ExtData = string.Empty;

      return fields;
    }



    static private FixedList<BillTaxEntryFields> MapToBillTaxFields(FixedList<SATBillTaxDto> impuestos) {

      List<BillTaxEntryFields> fields = new List<BillTaxEntryFields>();

      foreach (SATBillTaxDto tax in impuestos) {

        BillTaxEntryFields field = new BillTaxEntryFields();
        field.TaxType = tax.MetodoAplicacion;
        field.TaxFactorType = GetFactorTypeByTax(tax.TipoFactor);
        field.Factor = tax.TasaOCuota;
        field.BaseAmount = tax.Base;
        field.Total = tax.Importe;

        field.BillUID = string.Empty;
        field.BillConceptUID = string.Empty;
        field.ExtData = string.Empty;

        fields.Add(field);
      }
      return fields.ToFixedList();
    }


    #endregion Private methods


    #region Helpers

    static private BillTaxFactorType GetFactorTypeByTax(string tipoFactor) {

      switch (tipoFactor) {
        
        case "Cuota":
          return BillTaxFactorType.Cuota;

        case "Tasa":
          return BillTaxFactorType.Tasa;
        
        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled bill tax factor type for '{tipoFactor}'.");
      }
    }

    #endregion Helpers


  } // class BillFieldsMapper

} // namespace Empiria.Billing.Adapters
