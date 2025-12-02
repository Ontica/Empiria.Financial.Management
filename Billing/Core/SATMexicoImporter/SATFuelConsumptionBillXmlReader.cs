/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing SATMexico Importer                 Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Service provider                        *
*  Type     : SATFuelConsumptionBillXmlReader            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Service used to read a bill as xml string and return a SATBillDto object.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Xml;

namespace Empiria.Billing.SATMexicoImporter {

  /// <summary></summary>
  public class SATFuelConsumptionBillXmlReader {

    private readonly SATFuelConsumptionBillDto _satBillDto;

    private readonly XmlDocument _xmlDocument;

    internal SATFuelConsumptionBillXmlReader(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      _satBillDto = new SATFuelConsumptionBillDto();

      _xmlDocument = new XmlDocument();

      _xmlDocument.LoadXml(xmlString);
    }

    #region Services

    internal SATFuelConsumptionBillDto ReadAsFuelConsumptionBillDto() {

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

          GenerateAddendaConsumptionData(node);
        }
      }

      return _satBillDto;
    }

    #endregion Services

    #region Helpers

    private void GenerateAddendaConsumptionData(XmlNode addendaNode) {

      _satBillDto.Addenda = new SATFuelConsumptionAddendaDto();
      List<SATBillConceptWithTaxDto> conceptsDto = new List<SATBillConceptWithTaxDto>();

      XmlNodeList conceptsNodes = addendaNode.FirstChild.FirstChild.ChildNodes;

      foreach (XmlNode conceptItem in conceptsNodes) {

        if (!conceptItem.Name.Equals("edr:Concepto"))
          Assertion.EnsureFailed("The 'edr:Concepto' payment complement node it doesnt exist.");

        conceptsDto.Add(GetConceptItemData(conceptItem));
      }

      _satBillDto.Addenda.AddendaConceptos = conceptsDto.ToFixedList();
    }


    private void GenerateComplementData(XmlNode complementNode) {

      foreach (XmlNode complementChild in complementNode.ChildNodes) {

        if (complementChild.Name.Equals("ecc12:EstadoDeCuentaCombustible")) {

          GetAccountStatementData(complementChild);

        } else if (complementChild.Name.Equals("tfd:TimbreFiscalDigital")) {

          GetDigitalTaxStampData(complementChild);
        }
      }
    }


    private void GenerateConceptsList(XmlNode conceptsNode) {

      var conceptosDto = new List<SATBillConceptWithTaxDto>();

      foreach (XmlNode conceptItem in conceptsNode.ChildNodes) {

        if (!conceptItem.Name.Equals("cfdi:Concepto")) {
          Assertion.EnsureFailed("The concepts node must contain only concepts.");
        }

        conceptosDto.Add(GetConceptItemData(conceptItem));

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


    private void GetAccountStatementData(XmlNode complementChild) {

      _satBillDto.DatosComplemento = GetAccountStatementNodeData(complementChild);

      var conceptsList = new List<FuelConsumptionComplementConceptDataDto>();

      foreach (XmlNode concept in complementChild.ChildNodes) {

        if (concept.Name.Equals("ecc12:Conceptos")) {
          conceptsList.AddRange(GetConceptData(concept.ChildNodes));
        }
      }

      _satBillDto.DatosComplemento.ComplementConcepts = conceptsList.ToFixedList();
    }


    private FuelConsumptionComplementDataDto GetAccountStatementNodeData(XmlNode complementChild) {

      return new FuelConsumptionComplementDataDto {
        Version = GetAttribute(complementChild, "Version"),
        TipoOperacion = GetAttribute(complementChild, "TipoOperacion"),
        NumeroDeCuenta = GetAttribute(complementChild, "NumeroDeCuenta"),
        SubTotal = GetAttribute<decimal>(complementChild, "SubTotal"),
        Total = GetAttribute<decimal>(complementChild, "Total")
      };
    }


    private string GetAttribute(XmlNode concept, string attributeName) {
      return Patcher.Patch(concept.Attributes[attributeName]?.Value, string.Empty);
    }


    private T GetAttribute<T>(XmlNode concept, string attributeName) {
      if (concept.Attributes[attributeName]?.Value == null) {
        return default;
      } else {
        return (T) Convert.ChangeType(concept.Attributes[attributeName]?.Value, typeof(T));
      }
    }


    private FixedList<FuelConsumptionComplementConceptDataDto> GetConceptData(
                                    XmlNodeList accountStatementsConceptsNodes) {

      var accountStatementsConceptsList = new List<FuelConsumptionComplementConceptDataDto>();

      foreach (XmlNode accountStatementConcept in accountStatementsConceptsNodes) {

        if (accountStatementConcept.Name.Equals("ecc12:ConceptoEstadoDeCuentaCombustible")) {

          var conceptNode = new FuelConsumptionComplementConceptDataDto {
            Identificador = GetAttribute(accountStatementConcept, "Identificador"),
            Rfc = GetAttribute(accountStatementConcept, "Rfc"),
            ClaveEstacion = GetAttribute(accountStatementConcept, "ClaveEstacion"),
            TipoCombustible = GetAttribute(accountStatementConcept, "TipoCombustible"),
            Unidad = GetAttribute(accountStatementConcept, "Unidad"),
            NombreCombustible = GetAttribute(accountStatementConcept, "NombreCombustible"),
            FolioOperacion = GetAttribute(accountStatementConcept, "FolioOperacion"),
            Fecha = GetAttribute<DateTime>(accountStatementConcept, "Fecha"),
            Cantidad = Convert.ToDecimal(accountStatementConcept.Attributes["Cantidad"].Value),
            ValorUnitario = Convert.ToDecimal(accountStatementConcept.Attributes["ValorUnitario"].Value),
            Importe = Convert.ToDecimal(accountStatementConcept.Attributes["Importe"].Value),
            Impuestos = GetTaxesByConcept(accountStatementConcept.FirstChild)
          };

          accountStatementsConceptsList.Add(conceptNode);
        }
      }

      return accountStatementsConceptsList.ToFixedList();
    }


    private SATBillConceptWithTaxDto GetConceptItemData(XmlNode conceptItem) {
      
      return new SATBillConceptWithTaxDto() {
        ClaveProdServ = GetAttribute(conceptItem, "ClaveProdServ"),
        ClaveUnidad = GetAttribute(conceptItem, "ClaveUnidad"),
        Cantidad = GetAttribute<decimal>(conceptItem, "Cantidad"),
        Unidad = GetAttribute(conceptItem, "Unidad"),
        NoIdentificacion = GetAttribute(conceptItem, "NoIdentificacion"),
        Descripcion = GetAttribute(conceptItem, "Descripcion"),
        ValorUnitario = GetAttribute<decimal>(conceptItem, "ValorUnitario"),
        Importe = GetAttribute<decimal>(conceptItem, "Importe"),
        ObjetoImp = GetAttribute(conceptItem, "ObjetoImp"),
        Impuestos = GenerateTaxesByConcept(conceptItem.ChildNodes.Item(0))
      };
    }


    private void GetDigitalTaxStampData(XmlNode timbre) {
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


    private FixedList<SATBillTaxDto> GetTaxesByConcept(XmlNode taxesNode) {

      if (!taxesNode.Name.Equals("ecc12:Traslados"))
        Assertion.EnsureFailed("The 'ecc12:Traslados' payment complement node it doesnt exist.");

      var taxesData = new List<SATBillTaxDto>();

      foreach (XmlNode taxType in taxesNode.ChildNodes) {

        taxesData.Add(GetTaxItem(taxType));
      }

      return taxesData.ToFixedList();
    }


    private SATBillTaxDto GetTaxItem(XmlNode taxNode) {

      var taxDto = new SATBillTaxDto();

      taxDto.Base = GetAttribute<decimal>(taxNode, "Base");
      taxDto.Impuesto = GetAttribute(taxNode, "Impuesto");
      taxDto.TipoFactor = GetAttribute(taxNode, "TipoFactor");
      taxDto.TasaOCuota = GetAttribute<decimal>(taxNode, "TasaOCuota");
      taxDto.Importe = GetAttribute<decimal>(taxNode, "Importe");

      if (taxNode.Name.Contains("Traslado")) {
        taxDto.MetodoAplicacion = BillTaxMethod.Traslado;

      } else if (taxNode.Name.Contains("Retencion")) {
        taxDto.MetodoAplicacion = BillTaxMethod.Retencion;

      } else {

        throw Assertion.EnsureNoReachThisCode($"Unhandled SAT tax type: {taxNode.Name}");
      }
      return taxDto;
    }


    private IEnumerable<SATBillTaxDto> GetTaxItems(XmlNode taxNode) {
      var taxesByConceptDto = new List<SATBillTaxDto>();

      foreach (XmlNode taxItem in taxNode.ChildNodes) {

        taxesByConceptDto.Add(GetTaxItem(taxItem));
      }

      return taxesByConceptDto;
    }

    #endregion Helpers

  } // class SATFuelConsumptionBillXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
