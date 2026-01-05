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

    private readonly SATBillGeneralDataXmlReader generalDataReader;

    internal SATFuelConsumptionBillXmlReader(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      _satBillDto = new SATFuelConsumptionBillDto();

      _xmlDocument = new XmlDocument();

      _xmlDocument.LoadXml(xmlString);

      generalDataReader = new SATBillGeneralDataXmlReader();
    }

    #region Services

    internal SATFuelConsumptionBillDto ReadAsFuelConsumptionBillDto() {

      XmlElement generalData = _xmlDocument.DocumentElement;
      XmlNodeList nodes = generalData.ChildNodes;

      _satBillDto.DatosGenerales = generalDataReader.GenerateGeneralData(generalData);

      foreach (XmlNode node in nodes) {

        if (node.Name == "cfdi:Emisor") {

          _satBillDto.Emisor = generalDataReader.GenerateSenderData(node);
        }
        if (node.Name == "cfdi:Receptor") {

          _satBillDto.Receptor = generalDataReader.GenerateReceiverData(node);
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

      XmlNodeList addendaNodes = addendaNode.FirstChild.ChildNodes;

      foreach (XmlNode node in addendaNodes) {

        if (node.Name.Equals("edr:Conceptos")) {

          GetAddendaConcepts(node);

        } else if (node.Name.Equals("edr:LeyendasOtros")) {

          GetAddendaLabels(node);

        }
      }
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

      var conceptosDto = new List<SATBillConceptDto>();

      foreach (XmlNode conceptItem in conceptsNode.ChildNodes) {

        if (!conceptItem.Name.Equals("cfdi:Concepto")) {
          Assertion.EnsureFailed("The concepts node must contain only concepts.");
        }

        conceptosDto.Add(GetConceptItemData(conceptItem));

      }
      _satBillDto.Conceptos = conceptosDto.ToFixedList();
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

        taxesByConceptDto.Add(GetTaxItem(taxItem));
      }

      return taxesByConceptDto;
    }


    private void GetAccountStatementData(XmlNode complementChild) {

      _satBillDto.DatosComplemento = GetAccountStatementNodeData(complementChild);

      var conceptsList = new List<FuelConsumptionComplementConceptDataDto>();

      foreach (XmlNode concept in complementChild.ChildNodes) {

        if (concept.Name.Equals("ecc12:Conceptos")) {
          conceptsList.AddRange(GetConceptData(concept.ChildNodes));
        }
      }

      _satBillDto.DatosComplemento.ComplementoConceptos = conceptsList.ToFixedList();
    }


    private FuelConsumptionComplementDataDto GetAccountStatementNodeData(XmlNode complementChild) {

      return new FuelConsumptionComplementDataDto {
        Version = generalDataReader.GetAttribute(complementChild, "Version"),
        TipoOperacion = generalDataReader.GetAttribute(complementChild, "TipoOperacion"),
        NumeroDeCuenta = generalDataReader.GetAttribute(complementChild, "NumeroDeCuenta"),
        SubTotal = generalDataReader.GetAttribute<decimal>(complementChild, "SubTotal"),
        Total = generalDataReader.GetAttribute<decimal>(complementChild, "Total")
      };
    }


    private void GetAddendaConcepts(XmlNode node) {

      List<SATBillConceptDto> conceptsDto = new List<SATBillConceptDto>();

      XmlNodeList conceptsNodes = node.ChildNodes;

      foreach (XmlNode conceptItem in conceptsNodes) {

        if (!conceptItem.Name.Equals("edr:Concepto"))
          Assertion.EnsureFailed("The 'edr:Concepto' payment complement node it doesnt exist.");

        conceptsDto.Add(GetConceptItemData(conceptItem));
      }

      _satBillDto.Addenda.AddendaConceptos = conceptsDto.ToFixedList();
    }


    private void GetAddendaLabels(XmlNode node) {

      List<string> labels = new List<string>();

      XmlNodeList labelNodes = node.ChildNodes;

      foreach (XmlNode label in labelNodes) {

        if (!label.Name.Equals("edr:Leyenda"))
          Assertion.EnsureFailed("The 'edr:Leyenda' payment complement node it doesnt exist.");

        labels.Add(label.InnerText);
      }
      _satBillDto.Addenda.AddendaLeyendas = labels.ToFixedList();
    }


    private FixedList<FuelConsumptionComplementConceptDataDto> GetConceptData(
                                    XmlNodeList accountStatementsConceptsNodes) {

      var accountStatementsConceptsList = new List<FuelConsumptionComplementConceptDataDto>();

      foreach (XmlNode accountStatementConcept in accountStatementsConceptsNodes) {

        if (accountStatementConcept.Name.Equals("ecc12:ConceptoEstadoDeCuentaCombustible")) {

          var conceptNode = new FuelConsumptionComplementConceptDataDto {
            Identificador = generalDataReader.GetAttribute(accountStatementConcept, "Identificador"),
            Rfc = generalDataReader.GetAttribute(accountStatementConcept, "Rfc"),
            ClaveEstacion = generalDataReader.GetAttribute(accountStatementConcept, "ClaveEstacion"),
            TipoCombustible = generalDataReader.GetAttribute(accountStatementConcept, "TipoCombustible"),
            Unidad = generalDataReader.GetAttribute(accountStatementConcept, "Unidad"),
            NombreCombustible = generalDataReader.GetAttribute(accountStatementConcept, "NombreCombustible"),
            FolioOperacion = generalDataReader.GetAttribute(accountStatementConcept, "FolioOperacion"),
            Fecha = generalDataReader.GetAttribute<DateTime>(accountStatementConcept, "Fecha"),
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


    private SATBillConceptDto GetConceptItemData(XmlNode conceptItem) {
      
      return new SATBillConceptDto() {
        ClaveProdServ = generalDataReader.GetAttribute(conceptItem, "ClaveProdServ"),
        ClaveUnidad = generalDataReader.GetAttribute(conceptItem, "ClaveUnidad"),
        Cantidad = generalDataReader.GetAttribute<decimal>(conceptItem, "Cantidad"),
        Unidad = generalDataReader.GetAttribute(conceptItem, "Unidad"),
        NoIdentificacion = generalDataReader.GetAttribute(conceptItem, "NoIdentificacion"),
        Descripcion = generalDataReader.GetAttribute(conceptItem, "Descripcion"),
        ValorUnitario = generalDataReader.GetAttribute<decimal>(conceptItem, "ValorUnitario"),
        Importe = generalDataReader.GetAttribute<decimal>(conceptItem, "Importe"),
        Descuento = generalDataReader.GetAttribute<decimal>(conceptItem, "Descuento"),
        ObjetoImp = generalDataReader.GetAttribute(conceptItem, "ObjetoImp"),
        Impuestos = GenerateTaxesByConcept(conceptItem.ChildNodes.Item(0))
      };
    }


    private void GetDigitalTaxStampData(XmlNode timbre) {
      if (!timbre.Name.Equals("tfd:TimbreFiscalDigital")) {
        Assertion.EnsureFailed("The 'tfd:TimbreFiscalDigital' does not exist.");
      }

      _satBillDto.SATComplemento = new SATBillComplementDto {
        Xmlns_Tfd = generalDataReader.GetAttribute(timbre, "xmlns:tfd"),
        Xmlns_Xsi = generalDataReader.GetAttribute(timbre, "xmlns:xsi"),
        Xsi_SchemaLocation = generalDataReader.GetAttribute(timbre, "xsi:schemaLocation"),
        Tfd_Version = generalDataReader.GetAttribute(timbre, "Version"),
        UUID = generalDataReader.GetAttribute(timbre, "UUID"),
        FechaTimbrado = generalDataReader.GetAttribute<DateTime>(timbre, "FechaTimbrado"),
        RfcProvCertif = generalDataReader.GetAttribute(timbre, "RfcProvCertif"),
        SelloCFD = generalDataReader.GetAttribute(timbre, "SelloCFD"),
        NoCertificadoSAT = generalDataReader.GetAttribute(timbre, "NoCertificadoSAT"),
        SelloSAT = generalDataReader.GetAttribute(timbre, "SelloSAT")
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

      taxDto.Base = generalDataReader.GetAttribute<decimal>(taxNode, "Base");
      taxDto.Impuesto = generalDataReader.GetAttribute(taxNode, "Impuesto");
      taxDto.TipoFactor = generalDataReader.GetAttribute(taxNode, "TipoFactor");
      taxDto.TasaOCuota = generalDataReader.GetAttribute<decimal>(taxNode, "TasaOCuota");
      taxDto.Importe = generalDataReader.GetAttribute<decimal>(taxNode, "Importe");

      taxDto.TipoFactor = taxDto.TipoFactor != string.Empty ? taxDto.TipoFactor : "None";

      if (taxNode.Name.Contains("Traslado")) {
        taxDto.MetodoAplicacion = BillTaxMethod.Traslado;

      } else if (taxNode.Name.Contains("Retencion")) {
        taxDto.MetodoAplicacion = BillTaxMethod.Retencion;

      } else {

        throw Assertion.EnsureNoReachThisCode($"Unhandled SAT tax type: {taxNode.Name}");
      }
      return taxDto;
    }

    #endregion Helpers

  } // class SATFuelConsumptionBillXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
