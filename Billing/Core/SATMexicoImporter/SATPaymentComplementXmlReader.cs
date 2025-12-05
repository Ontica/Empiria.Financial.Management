/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing SATMexico Importer                 Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Service provider                        *
*  Type     : SATPaymentComplementXmlReader              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Service used to read a payment complement as xml string and return a SATBillDto object.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Xml;
using System.Xml.Linq;

namespace Empiria.Billing.SATMexicoImporter {

  /// <summary>Service used to read a payment complement as xml string and return a SATBillDto object.</summary>
  internal class SATPaymentComplementXmlReader {

    private readonly SatBillPaymentComplementDto _satPaymentComplementDto;

    private readonly XmlDocument _xmlDocument;

    private readonly SATBillGeneralDataXmlReader generalDataReader;

    internal SATPaymentComplementXmlReader(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      _satPaymentComplementDto = new SatBillPaymentComplementDto();

      _xmlDocument = new XmlDocument();

      _xmlDocument.LoadXml(xmlString);

      generalDataReader = new SATBillGeneralDataXmlReader();
    }


    #region Services

    internal SatBillPaymentComplementDto ReadAsPaymentComplementDto() {

      XmlElement generalData = _xmlDocument.DocumentElement;
      XmlNodeList nodes = generalData.ChildNodes;

      _satPaymentComplementDto.DatosGenerales = generalDataReader.GenerateGeneralData(generalData);

      foreach (XmlNode node in nodes) {

        if (node.Name == "cfdi:Emisor") {

          _satPaymentComplementDto.Emisor = generalDataReader.GenerateSenderData(node);
        }
        if (node.Name == "cfdi:Receptor") {

          _satPaymentComplementDto.Receptor = generalDataReader.GenerateReceiverData(node);
        }
        if (node.Name == "cfdi:Conceptos") {

          GenerateConceptsList(node);
        }
        if (node.Name == "cfdi:Complemento") {

          GenerateComplementData(node);
        }
      }

      return _satPaymentComplementDto;
    }

    #endregion Services

    #region Helpers


    private void GenerateComplementData(XmlNode complementNode) {

      foreach (XmlNode complementChild in complementNode.ChildNodes) {

        if (complementChild.Name.Equals("pago20:Pagos")) {

          GetPaymentData(complementChild);
        } else if (complementChild.Name.Equals("tfd:TimbreFiscalDigital")) {

          GetDigitalTaxStampData(complementChild);
        }
      }
    }


    private void GetDigitalTaxStampData(XmlNode timbre) {

      if (!timbre.Name.Equals("tfd:TimbreFiscalDigital")) {
        Assertion.EnsureFailed("The 'tfd:TimbreFiscalDigital' does not exist.");
      }

      _satPaymentComplementDto.SATComplemento = new SATBillComplementDto {
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


    private void GetPaymentData(XmlNode pago20Pagos) {

      _satPaymentComplementDto.DatosComplemento = new PaymentComplementDataDto {
        PagosVersion = generalDataReader.GetAttribute(pago20Pagos, "Version")
      };

      var balancesDataList = new List<ComplementBalanceDataDto>();
      var payoutDataList = new List<ComplementRelatedPayoutDataDto>();

      foreach (XmlNode concept in pago20Pagos.ChildNodes) {

        if (concept.Name.Equals("pago20:Totales")) {

          balancesDataList.Add(GetBalancesData(concept));

        } else if (concept.Name.Equals("pago20:Pago")) {

          payoutDataList.Add(GetPayoutData(concept));
        }
      }
      _satPaymentComplementDto.DatosComplemento.SaldosTotales = balancesDataList.ToFixedList();
      _satPaymentComplementDto.DatosComplemento.DatosComplementoPago = payoutDataList.ToFixedList();

    }

    private void GenerateConceptsList(XmlNode conceptsNode) {

      var conceptosDto = new List<SATBillConceptDto>();

      foreach (XmlNode concept in conceptsNode.ChildNodes) {

        if (!concept.Name.Equals("cfdi:Concepto")) {
          Assertion.EnsureFailed("The concepts node must contain only concepts.");
        }

        var conceptoDto = new SATBillConceptDto() {
          ClaveProdServ = generalDataReader.GetAttribute(concept, "ClaveProdServ"),
          ClaveUnidad = generalDataReader.GetAttribute(concept, "ClaveUnidad"),
          Cantidad = generalDataReader.GetAttribute<decimal>(concept, "Cantidad"),
          Unidad = generalDataReader.GetAttribute(concept, "Unidad"),
          NoIdentificacion = generalDataReader.GetAttribute(concept, "NoIdentificacion"),
          Descripcion = generalDataReader.GetAttribute(concept, "Descripcion"),
          ValorUnitario = generalDataReader.GetAttribute<decimal>(concept, "ValorUnitario"),
          Importe = generalDataReader.GetAttribute<decimal>(concept, "Importe"),
          ObjetoImp = generalDataReader.GetAttribute(concept, "ObjetoImp"),
        };

        conceptosDto.Add(conceptoDto);

      }
      _satPaymentComplementDto.Conceptos = conceptosDto.ToFixedList();
    }


    private ComplementBalanceDataDto GetBalancesData(XmlNode concept) {

      var returnedBalance = new ComplementBalanceDataDto {
        TotalTrasladosBaseIVA16 = Convert.ToDecimal(concept.Attributes["TotalTrasladosBaseIVA16"].Value),
        TotalTrasladosImpuestoIVA16 = Convert.ToDecimal(concept.Attributes["TotalTrasladosImpuestoIVA16"].Value),
        MontoTotalPagos = Convert.ToDecimal(concept.Attributes["MontoTotalPagos"].Value),
      };
      return returnedBalance;
    }


    private ComplementRelatedPayoutDataDto GetPayoutData(XmlNode concept) {

      var payoutData = new ComplementRelatedPayoutDataDto {
        FechaPago = generalDataReader.GetAttribute<DateTime>(concept, "FechaPago"),
        FormaDePagoP = generalDataReader.GetAttribute(concept, "FormaDePagoP"),
        MonedaP = generalDataReader.GetAttribute(concept, "MonedaP"),
        TipoCambioP = generalDataReader.GetAttribute(concept, "TipoCambioP"),
        Monto = Convert.ToDecimal(concept.Attributes["Monto"].Value),
        NumOperacion = generalDataReader.GetAttribute(concept, "NumOperacion"),
        RelatedDocumentData = GetRelatedDocumentData(concept.ChildNodes)
      };

      return payoutData;
    }


    private FixedList<ComplementRelatedDocumentDataDto> GetRelatedDocumentData(XmlNodeList relatedDocNodes) {

      var relatedDocumentsList = new List<ComplementRelatedDocumentDataDto>();

      foreach (XmlNode relatedDocNode in relatedDocNodes) {

        if (relatedDocNode.Name.Equals("pago20:DoctoRelacionado")) {

          var relatedDoc = new ComplementRelatedDocumentDataDto {
            IdDocumento = generalDataReader.GetAttribute(relatedDocNode, "IdDocumento"),
            MonedaDR = generalDataReader.GetAttribute(relatedDocNode, "MonedaDR"),
            EquivalenciaDR = generalDataReader.GetAttribute(relatedDocNode, "EquivalenciaDR"),
            NumParcialidad = generalDataReader.GetAttribute(relatedDocNode, "NumParcialidad"),
            ImpSaldoAnt = generalDataReader.GetAttribute<decimal>(relatedDocNode, "ImpSaldoAnt"),
            ImpPagado = generalDataReader.GetAttribute<decimal>(relatedDocNode, "ImpPagado"),
            ImpSaldoInsoluto = generalDataReader.GetAttribute<decimal>(relatedDocNode, "ImpSaldoInsoluto"),
            ObjetoImpDR = generalDataReader.GetAttribute(relatedDocNode, "ObjetoImpDR"),
            Taxes = GetTaxes(relatedDocNode.FirstChild)
          };
          relatedDocumentsList.Add(relatedDoc);

        } else if (relatedDocNode.Name.Equals("pago20:ImpuestosP")) {
          //TODO ASK IF THIS INFORMATION IS REQUIRED
        }
      }

      return relatedDocumentsList.ToFixedList();
    }


    private FixedList<SATBillTaxDto> GetTaxes(XmlNode taxesNode) {

      if (!taxesNode.Name.Equals("pago20:ImpuestosDR"))
        Assertion.EnsureFailed("The 'pago20:ImpuestosDR' payment complement node it doesnt exist.");

      var taxesByRelatedDoc = new List<SATBillTaxDto>();

      foreach (XmlNode taxType in taxesNode.ChildNodes) {

        taxesByRelatedDoc.AddRange(GetTaxItems(taxType));
      }
      return taxesByRelatedDoc.ToFixedList();
    }


    private IEnumerable<SATBillTaxDto> GetTaxItems(XmlNode taxNode) {
      var taxesByRelatedDoc = new List<SATBillTaxDto>();

      foreach (XmlNode taxItem in taxNode.ChildNodes) {

        var taxDto = new SATBillTaxDto();
        taxDto.Base = Convert.ToDecimal(taxItem.Attributes["BaseDR"].Value);
        taxDto.Impuesto = taxItem.Attributes["ImpuestoDR"].Value;
        taxDto.TipoFactor = taxItem.Attributes["TipoFactorDR"].Value;
        taxDto.TasaOCuota = Convert.ToDecimal(taxItem.Attributes["TasaOCuotaDR"].Value);
        taxDto.Importe = Convert.ToDecimal(taxItem.Attributes["ImporteDR"].Value);

        if (taxItem.Name == "pago20:TrasladoDR") {
          taxDto.MetodoAplicacion = BillTaxMethod.Traslado;

        } else if (taxItem.Name == "pago20:RetencionDR") {
          taxDto.MetodoAplicacion = BillTaxMethod.Retencion;

        } else {
          throw Assertion.EnsureNoReachThisCode($"Unhandled SAT tax type: {taxItem.Name}");
        }
        taxesByRelatedDoc.Add(taxDto);
      }

      return taxesByRelatedDoc;
    }


    #endregion Helpers
  }
}
