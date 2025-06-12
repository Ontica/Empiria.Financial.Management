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

namespace Empiria.Financial {

  /// <summary>Holds credit financial account attributes.</summary>
  public class CreditAttributes : AccountAttributes {

    #region Constructors and Parsers

    private readonly JsonObject _attributes = new JsonObject();

    public CreditAttributes() {

    }

    internal CreditAttributes(JsonObject attributes) {
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


    public string AccountingAccount {
      get {
        return _attributes.Get("creditAccountingAccount", string.Empty);
      }
      internal set {
        _attributes.SetIfValue("creditAccountingAccount", value);
      }
    }


    public CreditStage CreditStage {
      get {
        return _attributes.Get("creditStageId", CreditStage.Empty);
      }
      internal set {
        _attributes.SetIfValue("creditStageId", value.Id);
      }
    }


    public CreditType CreditType {
      get {
        return _attributes.Get("creditTypeId", CreditType.Empty);
      }
      internal set {
        _attributes.SetIfValue("creditTypeId", value.Id);
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

    #endregion Properties

    #region Helpers

    internal override string ToJsonString() {
      return _attributes.ToString();
    }

    #endregion Helpers

  } // class CreditAttributes

} // namespace Empiria.Financial
