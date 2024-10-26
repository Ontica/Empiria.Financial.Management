/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing SATMexico Importer                 Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Service provider                        *
*  Type     : SATBillXmlReader                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Service used to read a bill as xml string and return a SATBillDto object.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Xml;

namespace Empiria.Billing.SATMexicoImporter {

  /// <summary>Service used to read a bill as xml string and return a SATBillDto object.</summary>
  internal class SATBillXmlReader {

    private readonly SATBillDto _satBillDto;

    private readonly XmlDocument _xmlDocument;

    internal SATBillXmlReader(string xmlFilePath) {
      Assertion.Require(xmlFilePath, nameof(xmlFilePath));

      _satBillDto = new SATBillDto();

      _xmlDocument = new XmlDocument();
      _xmlDocument.Load(xmlFilePath);
    }

    #region Services

    internal SATBillDto ReadAsBillDto() {

      XmlElement generalData = _xmlDocument.DocumentElement;
      XmlNodeList nodes = generalData.ChildNodes;

      GenerateGeneralData(generalData);

      foreach (XmlNode node in nodes) {

        if (node.Name == "cfdi:Emisor") {

          GenerateSenderData(node);
        }
        if (node.Name == "cfdi:Receptor") {

          GenerateReceiverData(node);
        }
        if (node.Name == "cfdi:Conceptos") {

          GenerateConceptsList(node);
        }
      }

      return _satBillDto;
    }

    #endregion Services

    #region Helpers

    private void GenerateConceptsList(XmlNode conceptsHeader) {

      var conceptosDto = new List<SATBillConceptDto>();

      foreach (XmlNode concept in conceptsHeader.ChildNodes) {

        if (!concept.Name.Equals("cfdi:Concepto")) {
          Assertion.EnsureFailed("The concepts node must contain only concepts.");
        }

        var conceptoDto = new SATBillConceptDto();
        conceptoDto.ClaveProdServ = concept.Attributes["ClaveProdServ"].Value;
        conceptoDto.ClaveUnidad = concept.Attributes["ClaveUnidad"].Value;
        conceptoDto.Cantidad = Convert.ToDecimal(concept.Attributes["Cantidad"].Value);
        conceptoDto.Unidad = concept.Attributes["Unidad"].Value;
        conceptoDto.NoIdentificacion = concept.Attributes["NoIdentificacion"].Value;
        conceptoDto.Descripcion = concept.Attributes["Descripcion"].Value;
        conceptoDto.ValorUnitario = Convert.ToDecimal(concept.Attributes["ValorUnitario"].Value);
        conceptoDto.Importe = Convert.ToDecimal(concept.Attributes["Importe"].Value);
        conceptoDto.ObjetoImp = concept.Attributes["ObjetoImp"].Value;

        conceptoDto.Impuestos = GenerateTaxesByConcept(concept.ChildNodes.Item(0));
        conceptosDto.Add(conceptoDto);

      }
      _satBillDto.Conceptos = conceptosDto.ToFixedList();
    }


    private void GenerateGeneralData(XmlElement generalData) {

      if (generalData.Name != "cfdi:Comprobante") {
        Assertion.EnsureFailed("El archivo xml no es un cfdi.");
      } else if (!generalData.GetAttribute("Version").Equals("4.0")) {
        Assertion.EnsureFailed("The CFDI version is not correct.");
      }


      _satBillDto.DatosGenerales.CFDIVersion = generalData.GetAttribute("Version") ?? string.Empty;
      _satBillDto.DatosGenerales.Folio = generalData.GetAttribute("Folio") ?? string.Empty;
      _satBillDto.DatosGenerales.Fecha = Convert.ToDateTime(generalData.GetAttribute("Fecha"));
      _satBillDto.DatosGenerales.Sello = generalData.GetAttribute("Version") ?? string.Empty;
      _satBillDto.DatosGenerales.FormaPago = generalData.GetAttribute("FormaPago") ?? string.Empty;
      _satBillDto.DatosGenerales.NoCertificado = generalData.GetAttribute("NoCertificado") ?? string.Empty;
      _satBillDto.DatosGenerales.Certificado = generalData.GetAttribute("Certificado") ?? string.Empty;

      _satBillDto.DatosGenerales.SubTotal = Convert.ToDecimal(generalData.GetAttribute("SubTotal"));
      _satBillDto.DatosGenerales.Moneda = generalData.GetAttribute("Moneda") ?? string.Empty;
      _satBillDto.DatosGenerales.Total = Convert.ToDecimal(generalData.GetAttribute("Total"));

      _satBillDto.DatosGenerales.TipoDeComprobante = generalData.GetAttribute("TipoDeComprobante") ?? string.Empty;
      _satBillDto.DatosGenerales.Exportacion = generalData.GetAttribute("Exportacion") ?? string.Empty;
      _satBillDto.DatosGenerales.MetodoPago = generalData.GetAttribute("MetodoPago") ?? string.Empty;
      _satBillDto.DatosGenerales.LugarExpedicion = generalData.GetAttribute("LugarExpedicion") ?? string.Empty;
    }


    private void GenerateReceiverData(XmlNode receiver) {

      _satBillDto.Receptor.RegimenFiscal = receiver.Attributes["RegimenFiscalReceptor"].Value ?? string.Empty;
      _satBillDto.Receptor.RFC = receiver.Attributes["Rfc"].Value ?? string.Empty;
      _satBillDto.Receptor.Nombre = receiver.Attributes["Nombre"].Value ?? string.Empty;
      _satBillDto.Receptor.DomicilioFiscal = receiver.Attributes["DomicilioFiscalReceptor"].Value ?? string.Empty;
      _satBillDto.Receptor.UsoCFDI = receiver.Attributes["UsoCFDI"].Value ?? string.Empty;
    }


    private void GenerateSenderData(XmlNode sender) {

      _satBillDto.Emisor.RegimenFiscal = sender.Attributes["RegimenFiscal"].Value ?? string.Empty;
      _satBillDto.Emisor.RFC = sender.Attributes["Rfc"].Value ?? string.Empty;
      _satBillDto.Emisor.Nombre = sender.Attributes["Nombre"].Value ?? string.Empty;
    }


    private FixedList<SATBillTaxDto> GenerateTaxesByConcept(XmlNode taxesNode) {

      var taxesByConceptDto = new List<SATBillTaxDto>();

      foreach (XmlNode taxNode in taxesNode.ChildNodes) {

        taxesByConceptDto.AddRange(GetTaxItems(taxNode));
      }
      return taxesByConceptDto.ToFixedList();
    }


    private IEnumerable<SATBillTaxDto> GetTaxItems(XmlNode taxNode) {
      var taxesByConceptDto = new List<SATBillTaxDto>();

      foreach (XmlNode taxItem in taxNode.ChildNodes) {

        var taxDto = new SATBillTaxDto();

        taxDto.Base = Convert.ToDecimal(taxItem.Attributes["Base"].Value);
        taxDto.Impuesto = taxItem.Attributes["Impuesto"].Value;
        taxDto.TipoFactor = taxItem.Attributes["TipoFactor"].Value;
        taxDto.TasaOCuota = Convert.ToDecimal(taxItem.Attributes["TasaOCuota"].Value);
        taxDto.Importe = Convert.ToDecimal(taxItem.Attributes["Importe"].Value);

        if (taxItem.Name == "cfdi:Traslado") {
          taxDto.MetodoAplicacion = BillTaxMethod.Traslado;

        } else if (taxItem.Name == "cfdi:Retencion") {
          taxDto.MetodoAplicacion = BillTaxMethod.Retencion;

        } else {

          throw Assertion.EnsureNoReachThisCode($"Unhandled SAT tax type: {taxNode.Name}");
        }
        taxesByConceptDto.Add(taxDto);
      }

      return taxesByConceptDto;
    }

    #endregion Helpers

  } // class SATBillXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
