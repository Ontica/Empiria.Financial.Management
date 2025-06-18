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

    internal SATPaymentComplementXmlReader(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      _satPaymentComplementDto = new SatBillPaymentComplementDto();

      _xmlDocument = new XmlDocument();

      _xmlDocument.LoadXml(xmlString);
    }


    #region Services

    internal SatBillPaymentComplementDto ReadAsPaymentComplementDto() {

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


    private void GetPaymentData(XmlNode pago20Pagos) {

      _satPaymentComplementDto.DatosComplemento = new PaymentComplementDataDto {
        PagosVersion = GetAttribute(pago20Pagos, "Version")
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
          ClaveProdServ = GetAttribute(concept, "ClaveProdServ"),
          ClaveUnidad = GetAttribute(concept, "ClaveUnidad"),
          Cantidad = GetAttribute<decimal>(concept, "Cantidad"),
          Unidad = GetAttribute(concept, "Unidad"),
          NoIdentificacion = GetAttribute(concept, "NoIdentificacion"),
          Descripcion = GetAttribute(concept, "Descripcion"),
          ValorUnitario = GetAttribute<decimal>(concept, "ValorUnitario"),
          Importe = GetAttribute<decimal>(concept, "Importe"),
          ObjetoImp = GetAttribute(concept, "ObjetoImp"),
        };

        conceptosDto.Add(conceptoDto);

      }
      _satPaymentComplementDto.Conceptos = conceptosDto.ToFixedList();
    }


    private void GenerateGeneralData(XmlElement generalDataNode) {

      if (!generalDataNode.Name.Equals("cfdi:Comprobante")) {
        Assertion.EnsureFailed("The xml file is not a valid CFDI document.");
      } else if (!generalDataNode.GetAttribute("Version").Equals("4.0")) {
        Assertion.EnsureFailed("The CFDI version is not correct.");
      }

      _satPaymentComplementDto.DatosGenerales = new SATBillGeneralDataDto {
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

      _satPaymentComplementDto.Receptor = new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(receiverNode, "RegimenFiscalReceptor"),
        RFC = GetAttribute(receiverNode, "Rfc"),
        Nombre = GetAttribute(receiverNode, "Nombre"),
        DomicilioFiscal = GetAttribute(receiverNode, "DomicilioFiscalReceptor"),
        UsoCFDI = GetAttribute(receiverNode, "UsoCFDI"),
      };
    }


    private void GenerateSenderData(XmlNode senderNode) {

      _satPaymentComplementDto.Emisor = new SATBillOrganizationDto {
        RegimenFiscal = GetAttribute(senderNode, "RegimenFiscal"),
        RFC = GetAttribute(senderNode, "Rfc"),
        Nombre = GetAttribute(senderNode, "Nombre"),
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
        FechaPago = GetAttribute<DateTime>(concept, "FechaPago"),
        FormaDePagoP = GetAttribute(concept, "FormaDePagoP"),
        MonedaP = GetAttribute(concept, "MonedaP"),
        TipoCambioP = GetAttribute(concept, "TipoCambioP"),
        Monto = Convert.ToDecimal(concept.Attributes["Monto"].Value),
        NumOperacion = GetAttribute(concept, "NumOperacion"),
        RelatedDocumentData = GetRelatedDocumentData(concept.ChildNodes)
      };

      return payoutData;
    }


    private FixedList<ComplementRelatedDocumentDataDto> GetRelatedDocumentData(XmlNodeList relatedDocNodes) {

      var relatedDocumentsList = new List<ComplementRelatedDocumentDataDto>();

      foreach (XmlNode relatedDocNode in relatedDocNodes) {

        if (relatedDocNode.Name.Equals("pago20:DoctoRelacionado")) {

          var relatedDoc = new ComplementRelatedDocumentDataDto {
            IdDocumento = GetAttribute(relatedDocNode, "IdDocumento"),
            MonedaDR = GetAttribute(relatedDocNode, "MonedaDR"),
            EquivalenciaDR = GetAttribute(relatedDocNode, "EquivalenciaDR"),
            NumParcialidad = GetAttribute(relatedDocNode, "NumParcialidad"),
            ImpSaldoAnt = Convert.ToDecimal(relatedDocNode.Attributes["ImpSaldoAnt"].Value),
            ImpPagado = Convert.ToDecimal(relatedDocNode.Attributes["ImpPagado"].Value),
            ImpSaldoInsoluto = Convert.ToDecimal(relatedDocNode.Attributes["ImpSaldoInsoluto"].Value),
            ObjetoImpDR = GetAttribute(relatedDocNode, "ObjetoImpDR"),
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
