/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Mapping class                           *
*  Type     : FuelConsumptionBillMapper                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for fuel consumption bill fields.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Vml;
using Empiria.Billing.SATMexicoImporter;
using Empiria.Parties;
using Empiria.Products.SATMexico;

namespace Empiria.Billing.Adapters {

  /// <summary>Mapping methods for fuel consumption bill fields</summary>
  static internal class FuelConsumptionBillMapper {

    #region Public methods

    static internal IBillFields Map(SATFuelConsumptionBillDto paymentComplementDto) {

      return MapToFuelConsumptionBillFields(paymentComplementDto);
    }

    #endregion Public methods


    #region Private methods

    static private FuelConsumptionBillAddendaFields MapToAddendaData(SATFuelConsumptionAddendaDto addenda) {

      return new FuelConsumptionBillAddendaFields {
        Labels = new FixedList<string>(addenda.AddendaLeyendas),
        Concepts = MapToFuelConsumptionConceptFields(addenda.AddendaConceptos)
      };
    }


    private static FixedList<FuelConsumptionComplementConceptDataFields> MapToComplementConcepts(
                                    FixedList<FuelConsumptionComplementConceptDataDto> complementConcepts) {

      var conceptsListFields = new List<FuelConsumptionComplementConceptDataFields>();

      foreach (var concept in complementConcepts) {

        var fields = new FuelConsumptionComplementConceptDataFields {
          SATProductUID = string.Empty,
          ProductUID = string.Empty,
          Identificador = concept.Identificador,
          Rfc = concept.Rfc,
          ClaveEstacion = concept.ClaveEstacion,
          TipoCombustible = concept.TipoCombustible,
          Unidad = concept.Unidad,
          NombreCombustible = concept.NombreCombustible,
          FolioOperacion = concept.FolioOperacion,
          Fecha = concept.Fecha,
          Cantidad = concept.Cantidad,
          ValorUnitario = concept.ValorUnitario,
          Importe = concept.Importe,
          TaxEntries = MapToTaxFields(concept.Impuestos)
        };
        conceptsListFields.Add(fields);
      }

      return conceptsListFields.ToFixedList();
    }


    static private IBillFields MapToFuelConsumptionBillFields(SATFuelConsumptionBillDto dto) {

      return new FuelConsumptionBillFields {
        BillCategoryUID = BillCategory.FacturaConsumoCombustible.UID,
        BillNo = dto.SATComplemento.UUID,
        CertificationNo = dto.DatosGenerales.NoCertificado,
        IssuedByUID = Party.TryParseWithID(dto.Emisor.RFC)?.UID ?? string.Empty,
        IssuedToUID = Party.TryParseWithID(dto.Receptor.RFC)?.UID ?? string.Empty,
        CurrencyUID = SATMoneda.ParseWithCode(dto.DatosGenerales.Moneda).Currency.UID,
        Subtotal = dto.DatosComplemento.SubTotal,
        //Discount = dto.DatosGenerales.Descuento,
        Total = dto.DatosComplemento.Total,
        Concepts = MapToFuelConsumptionConceptFields(dto.Conceptos),
        SchemaData = MapToFuelConsumptionSchemaData(dto),
        SecurityData = MapToFuelConsumptionSecurityData(dto),
        ComplementData = MapToFuelConsumptionComplementData(dto.DatosComplemento),
        Addenda = MapToAddendaData(dto.Addenda)
      };
    }


    static private FuelConsumptionComplementDataFields MapToFuelConsumptionComplementData(
                                                        FuelConsumptionComplementDataDto datosComplementos) {

      return new FuelConsumptionComplementDataFields {
        Version = datosComplementos.Version,
        TipoOperacion = datosComplementos.TipoOperacion,
        NumeroDeCuenta = datosComplementos.NumeroDeCuenta,
        SubTotal = datosComplementos.SubTotal, 
        Total = datosComplementos.Total,
        ComplementConcepts = MapToComplementConcepts(datosComplementos.ComplementoConceptos)
      };
    }


    static private FixedList<BillConceptFields> MapToFuelConsumptionConceptFields(
                                                        FixedList<SATBillConceptDto> conceptos) {

      List<BillConceptFields> fields = new List<BillConceptFields>();

      foreach (var concepto in conceptos) {

        var field = new BillConceptFields {
          ProductUID = string.Empty,
          SATProductUID = string.Empty,
          SATProductServiceCode = concepto.ClaveProdServ,
          UnitKey = concepto.ClaveUnidad,
          Unit = concepto.Unidad,
          IdentificationNo = concepto.NoIdentificacion,
          ObjectImp = concepto.ObjetoImp,
          Description = concepto.Descripcion,
          Quantity = concepto.Cantidad,
          UnitPrice = concepto.ValorUnitario,
          Subtotal = concepto.Importe,
          Discount = concepto.Descuento,
          IsBonusConcept = concepto.IsBonusConcept,
          TaxEntries = MapToTaxFields(concepto.Impuestos)
        };
        fields.Add(field);
      }
      return fields.ToFixedList();
    }


    static private BillSchemaDataFields MapToFuelConsumptionSchemaData(SATFuelConsumptionBillDto dto) {

      return new BillSchemaDataFields() {
        IssuedBy = MapToIssuedBy(dto.Emisor),
        IssuedTo = MapToIssuedTo(dto.Receptor),
        SchemaVersion = dto.DatosGenerales.CFDIVersion,
        Fecha = dto.DatosGenerales.Fecha,
        Folio = dto.DatosGenerales.Folio,
        Serie = dto.DatosGenerales.Serie,
        MetodoPago = dto.DatosGenerales.MetodoPago,
        FormaPago = dto.DatosGenerales.FormaPago,
        Exportacion = dto.DatosGenerales.Exportacion,
        LugarExpedicion = dto.DatosGenerales.LugarExpedicion,
        Moneda = dto.DatosGenerales.Moneda,
        Subtotal = dto.DatosComplemento.SubTotal,
        //Descuento = dto.DatosGenerales.Descuento,
        Total = dto.DatosComplemento.Total,
        TipoCambio = dto.DatosGenerales.TipoCambio,
        TipoComprobante = dto.DatosGenerales.TipoDeComprobante,
      };
    }


    static private BillSecurityDataFields MapToFuelConsumptionSecurityData(SATFuelConsumptionBillDto dto) {

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


    static private FixedList<BillTaxEntryFields> MapToTaxFields(FixedList<SATBillTaxDto> impuestos) {

      List<BillTaxEntryFields> fields = new List<BillTaxEntryFields>();

      foreach (SATBillTaxDto tax in impuestos) {

        var field = new BillTaxEntryFields {
          TaxMethod = tax.MetodoAplicacion,
          TaxFactorType = BillTaxEntryFields.GetFactorTypeByTax(tax.TipoFactor),
          Factor = tax.TasaOCuota,
          BaseAmount = tax.Base,
          Impuesto = tax.Impuesto,
          Total = tax.Importe,
          IsBonusTax = tax.IsBonusTax
        };

        fields.Add(field);
      }
      return fields.ToFixedList();
    }

    #endregion Private methods

  } // class FuelConsumptionBillMapper

} // namespace Empiria.Billing.Adapters
