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
using System.Linq;

using Empiria.Billing.SATMexicoImporter;

using Empiria.Parties;
using Empiria.Products.SATMexico;

namespace Empiria.Billing.Adapters {

  /// <summary>Mapping methods for bill fields.</summary>
  static internal class BillFieldsMapper {

    #region Public methods

    static internal IBillFields Map(SATBillDto satTBillDto) {

      return MapToBillFields(satTBillDto);
    }

    #endregion Public methods

    #region Private methods

    private static FixedList<BillTaxEntryFields> MapLocalTaxesToBillTaxes(
                                                  FixedList<BillComplementLocalTax> impuestosLocales) {
      List<BillTaxEntryFields> fields = new List<BillTaxEntryFields>();

      foreach (BillComplementLocalTax tax in impuestosLocales) {

        var field = new BillTaxEntryFields {
          TaxMethod = tax.MetodoAplicacion,
          TaxFactorType = BillTaxFactorType.None,
          Impuesto = tax.Impuesto,
          Description = tax.ImpLocalDescripcion,
          Factor = tax.TasaDe,
          Total = tax.Importe
        };

        fields.Add(field);
      }
      return fields.ToFixedList();
    }


    static private BillAddendaFields MapToAddendaData(SATBillAddenda addenda) {

      BillAddendaFields addendaFields = new BillAddendaFields();

      if (addenda != null) {

        addendaFields.MapEcoConceptFields(addenda);

        if (addenda.Concepto != null) {

          addendaFields.Folio = addenda.Folio;
          addendaFields.Serie = addenda.Serie;
          addendaFields.FechaEmision = addenda.FechaEmision;

          List<BillConceptFields> conceptFields = new List<BillConceptFields>();

          conceptFields.Add(MapToBillConceptFields(addenda.Concepto, true));
          addendaFields.Concepts = conceptFields.ToFixedList();
        }
      }

      return addendaFields;
    }


    static private BillConceptFields MapToBillConceptFields(SATBillConceptDto concepto,
                                                            bool addendaConcept = false) {

      return new BillConceptFields {
        ProductUID = string.Empty,
        SATProductUID = string.Empty,
        SATProductServiceCode = concepto.ClaveProdServ,
        UnitKey = concepto.ClaveUnidad,
        Unit = concepto.Unidad,
        IdentificationNo = concepto.NoIdentificacion,
        ObjectImp = concepto.ObjetoImp,
        Description = concepto.Descripcion,
        Quantity = concepto.Cantidad,
        UnitPrice = addendaConcept ? concepto.Cantidad * concepto.Importe : concepto.ValorUnitario,
        Subtotal = concepto.Importe,
        Discount = concepto.Descuento,
        TaxEntries = MapToBillTaxFields(concepto.Impuestos)
      };
    }


    static private FixedList<BillConceptFields> MapToBillConceptsFieldsList(
                                                FixedList<SATBillConceptDto> conceptos) {
      List<BillConceptFields> fields = new List<BillConceptFields>();

      foreach (var concepto in conceptos) {

        var field = MapToBillConceptFields(concepto);

        fields.Add(field);
      }
      return fields.ToFixedList();
    }


    static private IBillFields MapToBillFields(SATBillDto dto) {

      return new BillFields {
        BillCategoryUID = BillCategory.FacturaProveedores.UID,
        BillNo = dto.SATComplemento.UUID,
        CertificationNo = dto.DatosGenerales.NoCertificado,
        IssuedByUID = Party.TryParseWithID(dto.Emisor.RFC)?.UID ?? string.Empty,
        IssuedToUID = Party.TryParseWithID(dto.Receptor.RFC)?.UID ?? string.Empty,
        CurrencyUID = SATMoneda.ParseWithCode(dto.DatosGenerales.Moneda).Currency.UID,
        Subtotal = dto.DatosGenerales.SubTotal,
        Discount = dto.DatosGenerales.Descuento,
        Total = dto.DatosGenerales.Total,
        Concepts = MapToBillConceptsFieldsList(dto.Conceptos),
        SchemaData = MapToSchemaData(dto),
        SecurityData = MapToSecurityData(dto),
        Addenda = MapToAddendaData(dto.Addenda),
        FiscalLegendsData = MapToFiscalLegendsData(dto.LeyendasFiscales),
        BillTaxes = MapLocalTaxesToBillTaxes(dto.ImpuestosLocales),
        BillRelatedDocument = MapToRelatedCfdi(dto.DatosGenerales.CfdiRelacionados),
      };
    }


    static private FixedList<BillTaxEntryFields> MapToBillTaxFields(FixedList<SATBillTaxDto> impuestos) {

      List<BillTaxEntryFields> fields = new List<BillTaxEntryFields>();

      foreach (SATBillTaxDto tax in impuestos) {

        var field = new BillTaxEntryFields {
          TaxMethod = tax.MetodoAplicacion,
          TaxFactorType = BillTaxEntryFields.GetFactorTypeByTax(tax.TipoFactor),
          Factor = tax.TasaOCuota,
          BaseAmount = tax.Base,
          Impuesto = tax.Impuesto,
          Total = tax.Importe
        };

        fields.Add(field);
      }
      return fields.ToFixedList();
    }


    private static BillFiscalLegendFields MapToFiscalLegendsData(
                                            FixedList<BillComplementFiscalLegend> fields) {
      if (fields.Count == 0) {
        return new BillFiscalLegendFields();
      }

      return new BillFiscalLegendFields {
        DisposicionFiscal = fields.First().DisposicionFiscal,
        Norma = fields.First().Norma,
        TextoLeyenda = fields.First().TextoLeyenda
      };
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


    static private BillRelatedDocumentData MapToRelatedCfdi(
                                            FixedList<SATBillCFDIRelatedDataDto> cfdiRelacionados) {

      if (cfdiRelacionados.Count == 0) {
        return new BillRelatedDocumentData();
      }

      return new BillRelatedDocumentData {
        RelatedCFDI = cfdiRelacionados.First().UUID,
        TipoRelacion = cfdiRelacionados.First().TipoRelacion,
        TipoRelacionNombre = MapToRelatedCfdiTypeName(cfdiRelacionados.First().TipoRelacion),
      };
    }


    static private BillSchemaDataFields MapToSchemaData(SATBillDto dto) {

      return new BillSchemaDataFields() {
        IssuedBy = MapToIssuedBy(dto.Emisor),
        IssuedTo = MapToIssuedTo(dto.Receptor),
        SchemaVersion = dto.DatosGenerales.CFDIVersion,
        Fecha = dto.DatosGenerales.Fecha,
        Folio = dto.DatosGenerales.Folio,
        Serie = dto.DatosGenerales.Serie,
        MetodoPago = dto.DatosGenerales.MetodoPago,
        FormaPago = dto.DatosGenerales.FormaPago,
        CondicionesPago = dto.DatosGenerales.CondicionesPago,
        Exportacion = dto.DatosGenerales.Exportacion,
        LugarExpedicion = dto.DatosGenerales.LugarExpedicion,
        Moneda = dto.DatosGenerales.Moneda,
        Subtotal = dto.DatosGenerales.SubTotal,
        Descuento = dto.DatosGenerales.Descuento,
        Total = dto.DatosGenerales.Total,
        TipoCambio = dto.DatosGenerales.TipoCambio,
        TipoComprobante = dto.DatosGenerales.TipoDeComprobante,
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

    static private BillRelatedDocumentType MapToRelatedCfdiTypeName(string tipoRelacion) {

      switch (tipoRelacion) {
        case "01":
          return BillRelatedDocumentType.NotaDeCredito;
        case "02":
          return BillRelatedDocumentType.NotaDeDebito;
        case "03":
          return BillRelatedDocumentType.DevolucionDeMercancia;
        case "04":
          return BillRelatedDocumentType.SustituciónDeCFDIPrevios;
        case "05":
          return BillRelatedDocumentType.TrasladoDeMercanciasFacturadasPreviamente;
        case "06":
          return BillRelatedDocumentType.FacturaGeneradaPorTrasladosPrevios;
        case "07":
          return BillRelatedDocumentType.AplicacionDeAnticipo;
        default:
          break;
      }
      return BillRelatedDocumentType.Ninguno;
    }
    
    #endregion Helpers

  } // class BillFieldsMapper

} // namespace Empiria.Billing.Adapters
