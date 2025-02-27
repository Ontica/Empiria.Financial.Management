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
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Xml;

namespace Empiria.Billing.SATMexicoImporter {

  /// <summary>Service used to read a bill as xml string and return a SATBillDto object.</summary>
  internal class SATBillXmlReader {

    private readonly SATBillDto _satBillDto;

    private readonly XmlDocument _xmlDocument;

    internal SATBillXmlReader(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      _satBillDto = new SATBillDto();

      _xmlDocument = new XmlDocument();

      _xmlDocument.LoadXml(xmlString);
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
        if (node.Name == "cfdi:Complemento") {

          GenerateComplementData(node);
        }
        if (node.Name == "cfdi:Addenda") {

          GenerateAddendaEcoComplementData(node);
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


    private void GenerateAddendaEcoComplementData(XmlNode complementNode) {

      XmlNode ecoComplement = complementNode.FirstChild;

      if (ecoComplement.Name.Equals("eco:Complementaria")) {

        _satBillDto.Addenda = new SATBillAddenda {
          NoEstacion = GetAttribute(ecoComplement, "noEstacion"),
          ClavePemex = GetAttribute(ecoComplement, "clavePemex"),
          EcoConcepts = GenerateAddendaEcoConcepts(ecoComplement.ChildNodes)
        };
      }
    }


    private FixedList<SATBillAddendaConcept> GenerateAddendaEcoConcepts(XmlNodeList ecoComplements) {

      var addendaConcepts = new List<SATBillAddendaConcept>();

      foreach (XmlNode ecoComplement in ecoComplements) {

        if (ecoComplement.Name.Equals("eco:Conceptos")) {

          XmlNode ecoConcept = ecoComplement.FirstChild;

          if (ecoConcept.Name.Equals("eco:Concepto")) {

            addendaConcepts.Add(
              new SATBillAddendaConcept {
                TasaIEPS = GetAttribute<decimal>(ecoConcept, "tasaIeps"),
                IEPS = GetAttribute<decimal>(ecoConcept, "ieps"),
                TasaIVA = GetAttribute<decimal>(ecoConcept, "tasaIva"),
                IVA = GetAttribute<decimal>(ecoConcept, "iva"),
                NoIdentificacion = GetAttribute<decimal>(ecoConcept, "noIdentificacion"),
                TasaAIEPS = GetAttribute<decimal>(ecoConcept, "tasaAieps"),
                AIEPS = GetAttribute<decimal>(ecoConcept, "aIeps")
              }
            );
          }
        }
      }
      return addendaConcepts.ToFixedList();
    }


    private void GenerateComplementData(XmlNode complementNode) {

      XmlNode timbre = complementNode.FirstChild;

      if (!timbre.Name.Equals("tfd:TimbreFiscalDigital")) {
        Assertion.EnsureFailed("The 'tfd:TimbreFiscalDigital' does not exist.");
      }

      _satBillDto.SATComplemento = new SATBillComplementDto {
        Xmlns_Tfd = GetAttribute(timbre, "xmlns:tfd"),
        Xmlns_Xsi = GetAttribute(timbre, "xmlns:xsi"),
        Xsi_SchemaLocation = GetAttribute(timbre, "xsi:schemaLocation"),
        Tfd_Version = GetAttribute(timbre, "Version"),
        UUID = GetAttribute(timbre, "UUID"),
        FechaTimbrado = GetAttribute<DateTime>(timbre, "FechaTimbrado"),
        RfcProvCertif = GetAttribute(timbre, "RfcProvCertif"),
        SelloCFD = GetAttribute(timbre, "SelloCFD"),
        NoCertificadoSAT = GetAttribute(timbre, "NoCertificadoSAT"),
        SelloSAT = GetAttribute(timbre, "SelloSAT")
      };
    }


    private void GenerateConceptsList(XmlNode conceptsNode) {

      var conceptosDto = new List<SATBillConceptWithTaxDto>();

      foreach (XmlNode concept in conceptsNode.ChildNodes) {

        if (!concept.Name.Equals("cfdi:Concepto")) {
          Assertion.EnsureFailed("The concepts node must contain only concepts.");
        }

        var conceptoDto = new SATBillConceptWithTaxDto() {
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


    private void GenerateGeneralData(XmlElement generalDataNode) {

      if (!generalDataNode.Name.Equals("cfdi:Comprobante")) {
        Assertion.EnsureFailed("El archivo xml no es un cfdi.");
      } else if (!generalDataNode.GetAttribute("Version").Equals("4.0")) {
        Assertion.EnsureFailed("The CFDI version is not correct.");
      }

      _satBillDto.DatosGenerales = new SATBillGeneralDataDto {
        CFDIVersion = GetAttribute(generalDataNode, "Version"),
        Folio = GetAttribute(generalDataNode, "Folio"),
        Fecha = GetAttribute<DateTime>(generalDataNode, "Fecha"),
        Sello = GetAttribute(generalDataNode, "Sello"),
        Serie = GetAttribute(generalDataNode, "Serie"),
        FormaPago = GetAttribute(generalDataNode, "FormaPago"),
        NoCertificado = GetAttribute(generalDataNode, "NoCertificado"),
        Certificado = GetAttribute(generalDataNode, "Certificado"),
        SubTotal = GetAttribute<decimal>(generalDataNode, "SubTotal"),
        Moneda = GetAttribute(generalDataNode, "Moneda"),
        Total = GetAttribute<decimal>(generalDataNode, "Total"),

        TipoDeComprobante = GetAttribute(generalDataNode, "TipoDeComprobante"),
        Exportacion = GetAttribute(generalDataNode, "Exportacion"),
        MetodoPago = GetAttribute(generalDataNode, "MetodoPago"),
        LugarExpedicion = GetAttribute(generalDataNode, "LugarExpedicion"),
      };

    }


    private void GenerateReceiverData(XmlNode receiverNode) {

      _satBillDto.Receptor = new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(receiverNode, "RegimenFiscalReceptor"),
        RFC = GetAttribute(receiverNode, "Rfc"),
        Nombre = GetAttribute(receiverNode, "Nombre"),
        DomicilioFiscal = GetAttribute(receiverNode, "DomicilioFiscalReceptor"),
        UsoCFDI = GetAttribute(receiverNode, "UsoCFDI"),
      };
    }


    private void GenerateSenderData(XmlNode senderNode) {

      _satBillDto.Emisor = new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(senderNode, "RegimenFiscal"),
        RFC = GetAttribute(senderNode, "Rfc"),
        Nombre = GetAttribute(senderNode, "Nombre"),
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

        taxDto.Base = GetAttribute<decimal>(taxItem, "Base");
        taxDto.Impuesto = GetAttribute(taxItem, "Impuesto");
        taxDto.TipoFactor = GetAttribute(taxItem, "TipoFactor");
        taxDto.TasaOCuota = GetAttribute<decimal>(taxItem, "TasaOCuota");
        taxDto.Importe = GetAttribute<decimal>(taxItem, "Importe");

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
