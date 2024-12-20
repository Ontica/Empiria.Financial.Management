﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Mapping class                           *
*  Type     : BillFieldsMapper                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for bill fields.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Parties;

using Empiria.Products.SATMexico;

using Empiria.Billing.SATMexicoImporter;
using System;
using System.Linq;

namespace Empiria.Billing.Adapters {

  /// <summary>Mapping methods for bill fields.</summary>
  static internal class BillFieldsMapper {

    #region Public methods

    static internal BillFields Map(SATBillDto satTBillDto) {

      return MapToBillFields(satTBillDto);
    }

    #endregion Public methods

    #region Private methods

    static private FixedList<BillConceptFields> MapToBillConceptFields(
                                                FixedList<SATBillConceptDto> conceptos) {
      List<BillConceptFields> fields = new List<BillConceptFields>();

      foreach (var concepto in conceptos) {

        var field = new BillConceptFields {
          ProductUID = string.Empty,
          SATProductCode = concepto.ClaveProdServ,
          ClaveUnidad = concepto.ClaveUnidad,
          Unidad = concepto.Unidad,
          NoIdentificacion = concepto.NoIdentificacion,
          ObjetoImp = concepto.ObjetoImp,
          Description = concepto.Descripcion,
          Quantity = concepto.Cantidad,
          UnitPrice = concepto.ValorUnitario,
          Subtotal = concepto.Importe,
          TaxEntries = MapToBillTaxFields(concepto.Impuestos)
        };
        fields.Add(field);
      }
      return fields.ToFixedList();
    }


    static private BillFields MapToBillFields(SATBillDto dto) {

      return new BillFields {
        BillCategoryUID = BillCategory.Factura.UID,
        BillNo = dto.SATComplemento.UUID,
        CertificationNo = dto.DatosGenerales.NoCertificado,
        IssueDate = dto.DatosGenerales.Fecha,
        IssuedByUID = Party.TryParseWithID(dto.Emisor.RFC)?.UID ?? string.Empty,
        IssuedToUID = Party.TryParseWithID(dto.Receptor.RFC)?.UID ?? string.Empty,
        SchemaVersion = dto.DatosGenerales.CFDIVersion,
        CurrencyUID = SATMoneda.ParseWithCode(dto.DatosGenerales.Moneda).Currency.UID,
        Subtotal = dto.DatosGenerales.SubTotal,
        Total = dto.DatosGenerales.Total,
        CFDIRelated = MapToCfdiRelated(dto.DatosGenerales.CfdiRelacionados),
        Concepts = MapToBillConceptFields(dto.Conceptos),
        SchemaData = MapToSchemaData(dto),
        SecurityData = MapToSecurityData(dto)
      };
    }


    static private FixedList<BillTaxEntryFields> MapToBillTaxFields(FixedList<SATBillTaxDto> impuestos) {

      List<BillTaxEntryFields> fields = new List<BillTaxEntryFields>();

      foreach (SATBillTaxDto tax in impuestos) {

        var field = new BillTaxEntryFields {
          TaxMethod = tax.MetodoAplicacion,
          TaxFactorType = GetFactorTypeByTax(tax.TipoFactor),
          Factor = tax.TasaOCuota,
          BaseAmount = tax.Base,
          Total = tax.Importe
        };

        fields.Add(field);
      }
      return fields.ToFixedList();
    }


    static private string MapToCfdiRelated(FixedList<SATBillCFDIRelatedDataDto> cfdiRelacionados) {

      if (cfdiRelacionados.Count == 0) {
        return string.Empty;
      }

      return cfdiRelacionados.First().UUID;
    }


    static private BillOrganizationFields MapToIssuedBy(SATBillOrganizationDto emisor) {

      return new BillOrganizationFields() {
        Nombre = emisor.Nombre,
        RFC = emisor.RFC,
        RegimenFiscal = emisor.RegimenFiscal
      };
    }


    static private BillOrganizationFields MapToIssuedTo(SATBillOrganizationDto receptor) {

      return new BillOrganizationFields() {
        Nombre = receptor.Nombre,
        RFC = receptor.RFC,
        RegimenFiscal = receptor.RegimenFiscal,
        DomicilioFiscal = receptor.DomicilioFiscal,
        UsoCFDI = receptor.UsoCFDI
      };
    }


    private static BillSchemaDataFields MapToSchemaData(SATBillDto dto) {

      return new BillSchemaDataFields() {
        IssuedBy = MapToIssuedBy(dto.Emisor),
        IssuedTo = MapToIssuedTo(dto.Receptor),
        TipoComprobante = dto.DatosGenerales.TipoDeComprobante,
        Folio = dto.DatosGenerales.Folio,
        MetodoPago = dto.DatosGenerales.MetodoPago,
        FormaPago = dto.DatosGenerales.FormaPago,
        Exportacion = dto.DatosGenerales.Exportacion,
        LugarExpedicion = dto.DatosGenerales.LugarExpedicion,
        Moneda = dto.DatosGenerales.Moneda
      };
    }


    static private BillSecurityDataFields MapToSecurityData(SATBillDto dto) {
      
      return new BillSecurityDataFields() {

        Xmlns_Xsi = dto.SATComplemento.Xmlns_Xsi,
        Xsi_SchemaLocation = dto.SATComplemento.Xsi_SchemaLocation,
        NoCertificado = dto.DatosGenerales.NoCertificado,
        Certificado = dto.DatosGenerales.Certificado,
        Sello = dto.DatosGenerales.Sello,

        UUID = dto.SATComplemento.UUID,
        SelloCFD = dto.SATComplemento.SelloCFD,
        SelloSAT = dto.SATComplemento.SelloSAT,
        FechaTimbrado = dto.SATComplemento.FechaTimbrado,
        RfcProvCertif = dto.SATComplemento.RfcProvCertif,
        NoCertificadoSAT = dto.SATComplemento.NoCertificadoSAT,
        Tfd_Version = dto.SATComplemento.Tfd_Version,
        Xmlns_Tfd = dto.SATComplemento.Xmlns_Tfd
      };
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
