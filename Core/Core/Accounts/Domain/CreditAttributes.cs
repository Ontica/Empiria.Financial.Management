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

    public CreditAttributes(ICreditAccountData account) {

      Borrower = account.CustomerName;
      //CreditStage = CreditStage.ParseNamedKey<CreditStage>(account.CreditStage);
      //CreditType = CreditType.ParseNamedKey<CreditType>(account.CreditType);
      ExternalCreditNo = account.ExternalCreditNo;
      SubledgerAccountNo = account.SubledgerAccountNo;
      CreditLineNo = account.CreditLineNo;
      CreditProjectType = account.CreditProjectType;
    }

    public CreditAttributes(JsonObject attributes) {
      Assertion.Require(attributes, nameof(attributes));

      _attributes = attributes;
    }

    #endregion Properties

    #region Properties

    public string Borrower {
      get {
        return _attributes.Get("borrower", string.Empty);
      }
      internal set {
        _attributes.SetIfValue("borrower", value);
      }
    }


    public string SubledgerAccountNo {
      get {
        return _attributes.Get("subledgerAccountNo", string.Empty);
      }
      internal set {
        _attributes.SetIfValue("subledgerAccountNo", value);
      }
    }


    public CreditProcessStage CreditProcessStage {
      get {
        return _attributes.Get("creditProcessStageId", CreditProcessStage.Empty);
      }
      internal set {
        _attributes.SetIf("creditProcessStageId", value.Id, !value.IsEmptyInstance);
      }
    }


    public CreditRiskStage CreditRiskStage {
      get {
        return _attributes.Get("creditRiskStageId", CreditRiskStage.Empty);
      }
      internal set {
        _attributes.SetIf("creditRiskStageId", value.Id, !value.IsEmptyInstance);
      }
    }


    public CreditType CreditType {
      get {
        return _attributes.Get("creditTypeId", CreditType.Empty);
      }
      internal set {
        _attributes.SetIf("creditTypeId", value.Id, !value.IsEmptyInstance);
      }
    }


    public CreditProjectType CreditProjectType {
      get {
        return _attributes.Get("creditProjectTypeId", CreditProjectType.Empty);
      }
      set {
        _attributes.SetIf("creditProjectTypeId", value.UID, !value.IsEmptyInstance);
      }
    }



    public string ExternalCreditNo {
      get {
        return _attributes.Get("externalCreditNo", string.Empty);
      }
      internal set {
        _attributes.SetIfValue("externalCreditNo", value);
      }
    }


    public string CreditLineNo {
      get {
        return _attributes.Get("creditLine", string.Empty);
      }
      internal set {
        _attributes.SetIfValue("creditLine", value);
      }
    }

    #endregion Properties

    #region Helpers

    internal JsonObject ToJson() {
      var json = JsonObject.Parse(_attributes.ToString());

      json.Set("CreditProjectType", CreditProjectType.MapToNamedEntity());

      return json;
    }

    public override string ToJsonString() {
      var json = JsonObject.Parse(_attributes.ToString());

      json.Set("CreditProjectType", CreditProjectType.MapToNamedEntity());

      return json.ToString();
    }

    #endregion Helpers

  } // class CreditAttributes

} // namespace Empiria.Financial
