/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : FinancialAccounts                          Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : CreditAttributes                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds credit financial account attributes.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

using Empiria.Financial.Adapters;

namespace Empiria.Financial {

  /// <summary>Holds credit financial account attributes.</summary>
  public class CreditAttributes : AccountAttributes {

    #region Constructors and Parsers

    private readonly JsonObject _attributes = new JsonObject();

    public CreditAttributes() {
      // no-op
    }


    public CreditAttributes(JsonObject attributes) {
      Assertion.Require(attributes, nameof(attributes));

      _attributes = attributes;

      PatchJsonUIDsFields();
    }


    public CreditAttributes(ICreditAccountData account) {

      Borrower = account.CustomerName;
      CreditType = account.CreditType;
      CreditProcessStage = account.CreditProcessStage;
      ExternalCreditNo = account.CreditNo;
      SubledgerAccountNo = account.SubledgerAccountNo;
      CreditLine = account.CreditLineNo;
      CreditProjectType = account.CreditProjectType;
      CreditRiskStage = account.CreditRiskStage;
    }

    #endregion Properties

    #region Properties

    public string Borrower {
      get {
        return _attributes.Get("borrower", string.Empty);
      }
      private set {
        _attributes.SetIfValue("borrower", value);
      }
    }


    public string SubledgerAccountNo {
      get {
        return _attributes.Get("subledgerAccountNo", string.Empty);
      }
      private set {
        _attributes.SetIfValue("subledgerAccountNo", value);
      }
    }


    public CreditProcessStage CreditProcessStage {
      get {
        return _attributes.Get("creditProcessStageId", CreditProcessStage.Empty);
      }
      private set {
        _attributes.SetIf("creditProcessStageId", value.Id, !value.IsEmptyInstance);
      }
    }


    public CreditRiskStage CreditRiskStage {
      get {
        return _attributes.Get("creditRiskStageId", CreditRiskStage.Empty);
      }
      private set {
        _attributes.SetIf("creditRiskStageId", value.Id, !value.IsEmptyInstance);
      }
    }


    public CreditType CreditType {
      get {
        return _attributes.Get("creditTypeId", CreditType.Empty);
      }
      private set {
        _attributes.SetIf("creditTypeId", value.Id, !value.IsEmptyInstance);
      }
    }


    public CreditProjectType CreditProjectType {
      get {
        return _attributes.Get("creditProjectTypeId", CreditProjectType.Empty);
      }
      private set {
        _attributes.SetIf("creditProjectTypeId", value.Id, !value.IsEmptyInstance);
      }
    }



    public string ExternalCreditNo {
      get {
        return _attributes.Get("externalCreditNo", string.Empty);
      }
      private set {
        _attributes.SetIfValue("externalCreditNo", value);
      }
    }


    public string CreditLine {
      get {
        return _attributes.Get("creditLine", string.Empty);
      }
      private set {
        _attributes.SetIfValue("creditLine", value);
      }
    }

    #endregion Properties

    #region Methods

    public JsonObject ToJson() {
      return _attributes;
    }

    public override string ToJsonString() {
      return _attributes.ToString();
    }

    #endregion Methods

    #region Helpers

    private void PatchJsonUIDsFields() {
      if (_attributes.HasValue("creditProcessStageUID")) {
        this.CreditProcessStage = _attributes.Get<CreditProcessStage>("creditProcessStageUID");
        _attributes.Remove("creditProcessStageUID");
      }
      if (_attributes.HasValue("creditRiskStageUID")) {
        this.CreditRiskStage = _attributes.Get<CreditRiskStage>("creditRiskStageUID");
        _attributes.Remove("creditRiskStageUID");
      }
      if (_attributes.HasValue("creditTypeUID")) {
        this.CreditType = _attributes.Get<CreditType>("creditTypeUID");
        _attributes.Remove("creditTypeUID");
      }
      if (_attributes.HasValue("creditProjectTypeUID")) {
        this.CreditProjectType = _attributes.Get<CreditProjectType>("creditProjectTypeUID");
        _attributes.Remove("creditProjectTypeUID");
      }
    }

    #endregion Helpers

  } // class CreditAttributes

} // namespace Empiria.Financial
