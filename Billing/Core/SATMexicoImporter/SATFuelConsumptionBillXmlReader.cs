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
using System.Linq;
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

          _satBillDto.Conceptos = generalDataReader.GenerateConceptsList(node);
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

      XmlNodeList addendaNodes = addendaNode.FirstChild.ChildNodes;

      foreach (XmlNode node in addendaNodes) {

        if (node.Name.Equals("edr:Conceptos")) {

          _satBillDto.Addenda.AddendaConceptos = generalDataReader.GenerateConceptsList(node);

          FilterAddendaBonusConcepts();

        } else if (node.Name.Equals("edr:LeyendasOtros")) {

          _satBillDto.Addenda.AddendaLeyendas = GetAddendaLabels(node);

        }
      }
    }

    private void FilterAddendaBonusConcepts() {

      var checkConcepts = new List<SATBillConceptDto>(_satBillDto.Addenda.AddendaConceptos);

      foreach (var concept in checkConcepts) {

        if (ValuateIfIsBonusConcept(concept) &&
            (concept.ValorUnitario == concept.Importe) &&
            concept.Cantidad == 1.00M) {

          concept.IsBonusConcept = true;
          concept.Impuestos.ToList().ForEach(x => x.IsBonusTax = true);
        } else {

          _satBillDto.Addenda.AddendaConceptos.Remove(concept);
        }
      }
    }


    private bool ValuateIfIsBonusConcept(SATBillConceptDto concept) {
      if ((concept.Descripcion.ToLower().StartsWith("bonif.") ||
          concept.Descripcion.ToLower().StartsWith("bonificacion") ||
          concept.Descripcion.ToLower().StartsWith("desc.") ||
          concept.Descripcion.ToLower().StartsWith("descuento") ||
          concept.Descripcion.ToLower().StartsWith("rebaja"))) {
        
        return true;
      }
      return false;
    }

    private void GenerateComplementData(XmlNode complementNode) {

      foreach (XmlNode complementChild in complementNode.ChildNodes) {

        if (complementChild.Name.Equals("tfd:TimbreFiscalDigital")) {

          _satBillDto.SATComplemento = generalDataReader.GenerateSATComplementData(complementChild);
        }
        if (complementChild.Name.Equals("ecc12:EstadoDeCuentaCombustible")) {

          GetAccountStatementData(complementChild);

        }
      }
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


    private FixedList<string> GetAddendaLabels(XmlNode node) {

      List<string> labels = new List<string>();

      XmlNodeList labelNodes = node.ChildNodes;

      foreach (XmlNode label in labelNodes) {

        if (!label.Name.Equals("edr:Leyenda"))
          Assertion.EnsureFailed("The 'edr:Leyenda' payment complement node it doesnt exist.");

        labels.Add(label.InnerText);
      }
      return labels.ToFixedList();
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


    private FixedList<SATBillTaxDto> GetTaxesByConcept(XmlNode taxesNode) {

      if (!taxesNode.Name.Equals("ecc12:Traslados"))
        Assertion.EnsureFailed("The 'ecc12:Traslados' payment complement node it doesnt exist.");

      var taxesData = new List<SATBillTaxDto>();

      foreach (XmlNode taxType in taxesNode.ChildNodes) {

        var tax = generalDataReader.GetTaxItem(taxType);

        if ((taxType.Name == "ecc12:Traslado" || taxType.Name == "ecc12:Retencion") &&
            tax.TipoFactor == "None") {
          tax.TipoFactor = "Tasa";
        }
        taxesData.Add(tax);
      }

      return taxesData.ToFixedList();
    }

    #endregion Helpers

  } // class SATFuelConsumptionBillXmlReader

} // namespace Empiria.Billing.SATMexicoImporter
