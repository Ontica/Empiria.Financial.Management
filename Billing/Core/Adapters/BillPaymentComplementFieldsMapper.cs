/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Mapping class                           *
*  Type     : BillPaymentComplementFieldsMapper          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for bill payment complement fields.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;
using Empiria.Billing.SATMexicoImporter;
using Empiria.Parties;
using Empiria.Products.SATMexico;

namespace Empiria.Billing.Adapters {

  /// <summary>Mapping methods for bill payment complement fields.</summary>
  static internal class BillPaymentComplementFieldsMapper {

    #region Public methods

    static internal IBillFields Map(SatBillPaymentComplementDto paymentComplementDto) {

      return MapToPaymentComplementFields(paymentComplementDto);
    }

    #endregion Public methods

    #region Private methods

    static private string MapToCfdiRelated(ComplementRelatedPayoutDataDto datosComplementoPago) {

      if (datosComplementoPago.RelatedDocumentData.Count == 0) {
        return string.Empty;
      }

      return datosComplementoPago.RelatedDocumentData.First().IdDocumento;
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


    static private IBillFields MapToPaymentComplementFields(SatBillPaymentComplementDto dto) {

      return new BillPaymentComplementFields {
        BillCategoryUID = BillCategory.ComplementoPagoProveedores.UID,
        BillNo = dto.SATComplemento.UUID,
        CertificationNo = dto.DatosGenerales.NoCertificado,
        IssuedByUID = Party.TryParseWithID(dto.Emisor.RFC)?.UID ?? string.Empty,
        IssuedToUID = Party.TryParseWithID(dto.Receptor.RFC)?.UID ?? string.Empty,
        CurrencyUID = SATMoneda.ParseWithCode(dto.DatosGenerales.Moneda).Currency.UID,
        Subtotal = dto.DatosGenerales.SubTotal,
        Total = dto.DatosGenerales.Total,
        //CFDIRelated = MapToCfdiRelated(dto.DatosComplemento.DatosComplementoPago.First()),
        Concepts = MapToPaymentComplementConceptFields(dto.Conceptos),
        SchemaData = MapToPaymentComplementSchemaData(dto),
        SecurityData = MapToPaymentComplementSecurityData(dto),
        ComplementRelatedPayoutData = MapToComplementRelatedPayout(dto.DatosComplemento.DatosComplementoPago)
      };
    }


    private static FixedList<ComplementRelatedPayoutDataFields> MapToComplementRelatedPayout(
      FixedList<ComplementRelatedPayoutDataDto> datosComplementoPagoDto) {
      var datosComplementos = new List<ComplementRelatedPayoutDataFields>();

      foreach (var complemento in datosComplementoPagoDto) {

        datosComplementos.Add(
          new ComplementRelatedPayoutDataFields {
            FechaPago = complemento.FechaPago,
            FormaDePagoP = complemento.FormaDePagoP,
            MonedaP = complemento.MonedaP,
            TipoCambioP = complemento.TipoCambioP,
            Monto = complemento.Monto,
            NumOperacion = complemento.NumOperacion,
            IdDocumento = complemento.RelatedDocumentData[0].IdDocumento,
            MonedaDR = complemento.RelatedDocumentData[0].MonedaDR,
            EquivalenciaDR = complemento.RelatedDocumentData[0].EquivalenciaDR,
            NumParcialidad = complemento.RelatedDocumentData[0].NumParcialidad,
            ImpSaldoAnt = complemento.RelatedDocumentData[0].ImpSaldoAnt,
            ImpPagado = complemento.RelatedDocumentData[0].ImpPagado,
            ImpSaldoInsoluto = complemento.RelatedDocumentData[0].ImpSaldoInsoluto,
            ObjetoImpDR = complemento.RelatedDocumentData[0].ObjetoImpDR,
            Taxes = MapToTaxes(complemento.RelatedDocumentData[0].Taxes)
          }
        );
      }
      return datosComplementos.ToFixedList();
    }


    private static FixedList<BillTaxEntryFields> MapToTaxes(FixedList<SATBillTaxDto> taxesDto) {
      var taxList = new List<BillTaxEntryFields>();

      foreach (var tax in taxesDto) {

        taxList.Add(
          new BillTaxEntryFields {
            TaxMethod = tax.MetodoAplicacion,
            TaxFactorType = BillTaxEntryFields.GetFactorTypeByTax(tax.TipoFactor),
            Factor = tax.TasaOCuota,
            BaseAmount = tax.Base,
            Total = tax.Importe,
            Impuesto = tax.Impuesto
          }
        );
      }
      return taxList.ToFixedList();
    }


    private static FixedList<BillConceptFields> MapToPaymentComplementConceptFields(
                                                  FixedList<SATBillConceptDto> conceptos) {
      var conceptFields = new List<BillConceptFields>();

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
          Subtotal = concepto.Importe
        };
        conceptFields.Add(field);
      }
      return conceptFields.ToFixedList();
    }


    private static BillSchemaDataFields MapToPaymentComplementSchemaData(SatBillPaymentComplementDto dto) {
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
        Subtotal = dto.DatosGenerales.SubTotal,
        Total = dto.DatosGenerales.Total,
        TipoComprobante = dto.DatosGenerales.TipoDeComprobante,
      };
    }


    private static BillSecurityDataFields MapToPaymentComplementSecurityData(
                                            SatBillPaymentComplementDto dto) {
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

  } // class BillPaymentComplementFieldsMapper

} // namespace Empiria.Billing.Adapters
