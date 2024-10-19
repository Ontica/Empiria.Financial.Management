/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Service provider                        *
*  Type     : BillXmlReader                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Service used to read a bill as xml string and return a BillDto object.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Xml;

using Empiria.Billing.Adapters;

namespace Empiria.Billing {

  /// <summary>Service used to read a bill as xml string and return a BillDto object.</summary>
  internal class BillXmlReader {

    static private XmlReader xmlReader;
    static private BillDto dto = new BillDto();

    #region Public methods


    static public BillDto ReadFromFilePath(string xmlFilePath) {

      xmlReader = XmlReader.Create(xmlFilePath);
      int count = 0;
      while (xmlReader.Read()) {

        if (count == 0) {
          ValidateXmlDocumentVersion();
        }

        GenerateGeneralData();

        GenerateSenderData();

        GenerateReceiverData();

        GenerateConceptsList();

        count++;
      }
      return dto;
    }


    private static void ValidateXmlDocumentVersion() {

      if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name != "cfdi:Comprobante")) {
        Assertion.EnsureNoReachThisCode($"El archivo xml no es una factura.");

      } else if (xmlReader.HasAttributes && xmlReader.GetAttribute("Version") != "4.0") {
        Assertion.EnsureNoReachThisCode($"La version del archivo xml no es la correcta.");
      }
    }


    #endregion Public methods

    #region Helpers

    static private void GenerateConceptsList() {

      if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "cfdi:Conceptos")) {

        var conceptosDto = new List<BillConceptDto>();

        XmlReader conceptosXml = xmlReader.ReadSubtree();

        while (conceptosXml.Read()) {

          if ((conceptosXml.NodeType == XmlNodeType.Element) && (conceptosXml.Name == "cfdi:Concepto")) {

            var conceptoDto = new BillConceptDto();

            conceptoDto.ClaveProdServ = conceptosXml.GetAttribute("ClaveProdServ");
            conceptoDto.ClaveUnidad = conceptosXml.GetAttribute("ClaveUnidad");
            conceptoDto.Cantidad = Convert.ToDecimal(conceptosXml.GetAttribute("Cantidad"));
            conceptoDto.Unidad = conceptosXml.GetAttribute("Unidad");
            conceptoDto.NoIdentificacion = conceptosXml.GetAttribute("NoIdentificacion");
            conceptoDto.Descripcion = conceptosXml.GetAttribute("Descripcion");
            conceptoDto.ValorUnitario = Convert.ToDecimal(conceptosXml.GetAttribute("ValorUnitario"));
            conceptoDto.Importe = Convert.ToDecimal(conceptosXml.GetAttribute("Importe"));
            conceptoDto.ObjetoImp = conceptosXml.GetAttribute("ObjetoImp");

            conceptoDto.Impuestos = GenerateTaxesByConcept(conceptosXml);
            conceptosDto.Add(conceptoDto);
          }
        }

        dto.Conceptos = conceptosDto.ToFixedList();
      }  // while
    }


    static private void GenerateGeneralData() {

      if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "cfdi:Comprobante")) {

        if (xmlReader.HasAttributes) {

          dto.DatosGenerales.CFDIVersion = xmlReader.GetAttribute("Version") ?? string.Empty;
          dto.DatosGenerales.Folio = xmlReader.GetAttribute("Folio") ?? string.Empty;
          dto.DatosGenerales.Fecha = Convert.ToDateTime(xmlReader.GetAttribute("Fecha"));
          dto.DatosGenerales.Sello = xmlReader.GetAttribute("Version") ?? string.Empty;
          dto.DatosGenerales.FormaPago = xmlReader.GetAttribute("FormaPago") ?? string.Empty;
          dto.DatosGenerales.NoCertificado = xmlReader.GetAttribute("NoCertificado") ?? string.Empty;
          dto.DatosGenerales.Certificado = xmlReader.GetAttribute("Certificado") ?? string.Empty;

          dto.DatosGenerales.SubTotal = Convert.ToDecimal(xmlReader.GetAttribute("SubTotal"));
          dto.DatosGenerales.Moneda = xmlReader.GetAttribute("Moneda") ?? string.Empty;
          dto.DatosGenerales.Total = Convert.ToDecimal(xmlReader.GetAttribute("Total"));

          dto.DatosGenerales.TipoDeComprobante = xmlReader.GetAttribute("TipoDeComprobante") ?? string.Empty;
          dto.DatosGenerales.Exportacion = xmlReader.GetAttribute("Exportacion") ?? string.Empty;
          dto.DatosGenerales.MetodoPago = xmlReader.GetAttribute("MetodoPago") ?? string.Empty;
          dto.DatosGenerales.LugarExpedicion = xmlReader.GetAttribute("LugarExpedicion") ?? string.Empty;
        }
      }

    }


    static private void GenerateReceiverData() {

      if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "cfdi:Receptor")) {

        if (xmlReader.HasAttributes) {
          dto.Receptor.RegimenFiscal = xmlReader.GetAttribute("RegimenFiscalReceptor") ?? string.Empty;
          dto.Receptor.RFC = xmlReader.GetAttribute("Rfc") ?? string.Empty;
          dto.Receptor.Nombre = xmlReader.GetAttribute("Nombre") ?? string.Empty;
          dto.Receptor.DomicilioFiscal = xmlReader.GetAttribute("DomicilioFiscalReceptor") ?? string.Empty;
          dto.Receptor.UsoCFDI = xmlReader.GetAttribute("UsoCFDI") ?? string.Empty;
        }
      }
    }


    static private void GenerateSenderData() {

      if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "cfdi:Emisor")) {

        if (xmlReader.HasAttributes) {

          dto.Emisor.RegimenFiscal = xmlReader.GetAttribute("RegimenFiscal") ?? string.Empty;
          dto.Emisor.RFC = xmlReader.GetAttribute("Rfc") ?? string.Empty;
          dto.Emisor.Nombre = xmlReader.GetAttribute("Nombre") ?? string.Empty;

        }

      }
    }


    static private FixedList<BillTaxDto> GenerateTaxesByConcept(XmlReader conceptosXml) {

      XmlReader impuestosXml = conceptosXml.ReadSubtree();

      var impuestosDto = new List<BillTaxDto>();

      while (impuestosXml.Read()) {

        if ((impuestosXml.NodeType == XmlNodeType.Element) &&
            (impuestosXml.Name == "cfdi:Traslado" || impuestosXml.Name == "cfdi:Retencion")) {

          var billTaxDto = new BillTaxDto();

          if (impuestosXml.Name == "cfdi:Traslado") {
            billTaxDto.TipoAplicacionImpuesto = BillTaxApplicationType.Traslado;
          } else if (impuestosXml.Name == "cfdi:Retencion") {
            billTaxDto.TipoAplicacionImpuesto = BillTaxApplicationType.Retencion;
          } else {
            throw Assertion.EnsureNoReachThisCode($"Unhandled SAT tax type: {impuestosXml.Name}");
          }

          billTaxDto.Base = Convert.ToDecimal(impuestosXml.GetAttribute("Base"));
          billTaxDto.Impuesto = impuestosXml.GetAttribute("Impuesto");
          billTaxDto.TipoFactor = impuestosXml.GetAttribute("TipoFactor");
          billTaxDto.TasaOCuota = Convert.ToDecimal(impuestosXml.GetAttribute("TasaOCuota"));
          billTaxDto.Importe = Convert.ToDecimal(impuestosXml.GetAttribute("Importe"));

          impuestosDto.Add(billTaxDto);
        }
      }  // while

      return impuestosDto.ToFixedList();
    }

    #endregion Helpers

  } // class BillXmlReader

} // namespace Empiria.Billing
