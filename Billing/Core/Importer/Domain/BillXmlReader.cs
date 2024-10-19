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

    private readonly XmlReader _xmlReader;
    private readonly BillDto _billDto;

    internal BillXmlReader(string xmlFilePath) {
      Assertion.Require(xmlFilePath, nameof(xmlFilePath));

      _xmlReader = XmlReader.Create(xmlFilePath);
      _billDto = new BillDto();
    }

    #region Services

    internal BillDto ReadAsBillDto() {

      int count = 0;

      while (_xmlReader.Read()) {

        if (count == 0) {
          ValidateXmlDocumentVersion();
        }

        GenerateGeneralData();

        GenerateSenderData();

        GenerateReceiverData();

        GenerateConceptsList();

        count++;
      }
      return _billDto;
    }


    private void ValidateXmlDocumentVersion() {

      if ((_xmlReader.NodeType == XmlNodeType.Element) && (_xmlReader.Name != "cfdi:Comprobante")) {
        Assertion.RequireFail($"El archivo xml no es una factura.");

      } else if (_xmlReader.HasAttributes && _xmlReader.GetAttribute("Version") != "4.0") {
        Assertion.RequireFail($"La version del archivo xml no es la correcta: {_xmlReader.GetAttribute("Version")}");
      }
    }


    #endregion Services

    #region Helpers

    private void GenerateConceptsList() {

      if ((_xmlReader.NodeType == XmlNodeType.Element) && (_xmlReader.Name == "cfdi:Conceptos")) {

        var conceptosDto = new List<BillConceptDto>();

        XmlReader conceptosXml = _xmlReader.ReadSubtree();

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

        _billDto.Conceptos = conceptosDto.ToFixedList();
      }  // while
    }


    private void GenerateGeneralData() {

      if ((_xmlReader.NodeType == XmlNodeType.Element) && (_xmlReader.Name == "cfdi:Comprobante")) {

        if (_xmlReader.HasAttributes) {

          _billDto.DatosGenerales.CFDIVersion = _xmlReader.GetAttribute("Version") ?? string.Empty;
          _billDto.DatosGenerales.Folio = _xmlReader.GetAttribute("Folio") ?? string.Empty;
          _billDto.DatosGenerales.Fecha = Convert.ToDateTime(_xmlReader.GetAttribute("Fecha"));
          _billDto.DatosGenerales.Sello = _xmlReader.GetAttribute("Version") ?? string.Empty;
          _billDto.DatosGenerales.FormaPago = _xmlReader.GetAttribute("FormaPago") ?? string.Empty;
          _billDto.DatosGenerales.NoCertificado = _xmlReader.GetAttribute("NoCertificado") ?? string.Empty;
          _billDto.DatosGenerales.Certificado = _xmlReader.GetAttribute("Certificado") ?? string.Empty;

          _billDto.DatosGenerales.SubTotal = Convert.ToDecimal(_xmlReader.GetAttribute("SubTotal"));
          _billDto.DatosGenerales.Moneda = _xmlReader.GetAttribute("Moneda") ?? string.Empty;
          _billDto.DatosGenerales.Total = Convert.ToDecimal(_xmlReader.GetAttribute("Total"));

          _billDto.DatosGenerales.TipoDeComprobante = _xmlReader.GetAttribute("TipoDeComprobante") ?? string.Empty;
          _billDto.DatosGenerales.Exportacion = _xmlReader.GetAttribute("Exportacion") ?? string.Empty;
          _billDto.DatosGenerales.MetodoPago = _xmlReader.GetAttribute("MetodoPago") ?? string.Empty;
          _billDto.DatosGenerales.LugarExpedicion = _xmlReader.GetAttribute("LugarExpedicion") ?? string.Empty;
        }
      }

    }


    private void GenerateReceiverData() {

      if ((_xmlReader.NodeType == XmlNodeType.Element) && (_xmlReader.Name == "cfdi:Receptor")) {

        if (_xmlReader.HasAttributes) {
          _billDto.Receptor.RegimenFiscal = _xmlReader.GetAttribute("RegimenFiscalReceptor") ?? string.Empty;
          _billDto.Receptor.RFC = _xmlReader.GetAttribute("Rfc") ?? string.Empty;
          _billDto.Receptor.Nombre = _xmlReader.GetAttribute("Nombre") ?? string.Empty;
          _billDto.Receptor.DomicilioFiscal = _xmlReader.GetAttribute("DomicilioFiscalReceptor") ?? string.Empty;
          _billDto.Receptor.UsoCFDI = _xmlReader.GetAttribute("UsoCFDI") ?? string.Empty;
        }
      }
    }


    private void GenerateSenderData() {

      if ((_xmlReader.NodeType == XmlNodeType.Element) && (_xmlReader.Name == "cfdi:Emisor")) {

        if (_xmlReader.HasAttributes) {

          _billDto.Emisor.RegimenFiscal = _xmlReader.GetAttribute("RegimenFiscal") ?? string.Empty;
          _billDto.Emisor.RFC = _xmlReader.GetAttribute("Rfc") ?? string.Empty;
          _billDto.Emisor.Nombre = _xmlReader.GetAttribute("Nombre") ?? string.Empty;

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
