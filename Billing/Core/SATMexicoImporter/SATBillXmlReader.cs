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

    private string GetAttribute(XmlNode concept, string attributeName) {
      return FieldPatcher.PatchField(concept.Attributes[attributeName]?.Value, string.Empty);
    }


    private T GetAttribute<T>(XmlNode concept, string attributeName) {
      if (concept.Attributes[attributeName]?.Value == null) {
        return default;
      } else {
        return (T) Convert.ChangeType(concept.Attributes[attributeName]?.Value, typeof(T));
      }
    }


    private void GenerateConceptsList(XmlNode conceptsHeader) {

      var conceptosDto = new List<SATBillConceptDto>();

      foreach (XmlNode concept in conceptsHeader.ChildNodes) {

        if (!concept.Name.Equals("cfdi:Concepto")) {
          Assertion.EnsureFailed("The concepts node must contain only concepts.");
        }

        var conceptoDto = new SATBillConceptDto() {
          ClaveProdServ = GetAttribute(concept, "ClaveProdServ"),
          ClaveUnidad = GetAttribute(concept, "ClaveUnidad"),
          Cantidad = GetAttribute<decimal>(concept, "Cantidad"),
          Unidad = GetAttribute(concept, "Unidad"),
          NoIdentificacion = GetAttribute(concept, "NoIdentificacion"),
          Descripcion = GetAttribute(concept, "Descripcion"),
          ValorUnitario = GetAttribute<decimal>(concept, "ValorUnitario"),
          Importe = GetAttribute<decimal>(concept, "Importe"),
          ObjetoImp = GetAttribute(concept, "ObjetoImp"),
          Impuestos = GenerateTaxesByConcept(concept.ChildNodes.Item(0))
        };

        conceptosDto.Add(conceptoDto);

      }
      _satBillDto.Conceptos = conceptosDto.ToFixedList();
    }


    private void GenerateGeneralData(XmlElement generalData) {

      if (!generalData.Name.Equals("cfdi:Comprobante")) {
        Assertion.EnsureFailed("El archivo xml no es un cfdi.");
      } else if (!generalData.GetAttribute("Version").Equals("4.0")) {
        Assertion.EnsureFailed("The CFDI version is not correct.");
      }

      _satBillDto.DatosGenerales = new SATBillGeneralDataDto {
        CFDIVersion = GetAttribute(generalData, "Version"),
        Folio = GetAttribute(generalData, "Folio"),
        Fecha = GetAttribute<DateTime>(generalData, "Fecha"),
        Sello = GetAttribute(generalData, "Version"),
        FormaPago = GetAttribute(generalData, "FormaPago"),
        NoCertificado = GetAttribute(generalData, "NoCertificado"),
        Certificado = GetAttribute(generalData, "Certificado"),

        SubTotal = GetAttribute<decimal>(generalData, "SubTotal"),
        Moneda = GetAttribute(generalData, "Moneda"),
        Total = GetAttribute<decimal>(generalData, "Total"),

        TipoDeComprobante = GetAttribute(generalData, "TipoDeComprobante"),
        Exportacion = GetAttribute(generalData, "Exportacion"),
        MetodoPago = GetAttribute(generalData, "MetodoPago"),
        LugarExpedicion = GetAttribute(generalData, "LugarExpedicion"),
      };

    }


    private void GenerateReceiverData(XmlNode receiver) {

      _satBillDto.Receptor = new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(receiver, "RegimenFiscalReceptor"),
        RFC = GetAttribute(receiver, "Rfc"),
        Nombre = GetAttribute(receiver, "Nombre"),
        DomicilioFiscal = GetAttribute(receiver, "DomicilioFiscalReceptor"),
        UsoCFDI = GetAttribute(receiver, "UsoCFDI"),
      };
    }


    private void GenerateSenderData(XmlNode sender) {

      _satBillDto.Emisor = new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(sender, "RegimenFiscal"),
        RFC = GetAttribute(sender, "Rfc"),
        Nombre = GetAttribute(sender, "Nombre"),
      };
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
