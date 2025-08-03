/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                            Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : FinancialRuleCategory                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial rule category.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Rules {

  /// <summary>Represents a financial rule category.</summary>
  public class FinancialRuleCategory : CommonStorage {

    #region Constructors and parsers

    protected FinancialRuleCategory() {
      // Required by Empiria Framework
    }

    static public FinancialRuleCategory Parse(int id) => ParseId<FinancialRuleCategory>(id);

    static public FinancialRuleCategory Parse(string uid) => ParseKey<FinancialRuleCategory>(uid);

    static public FinancialRuleCategory Empty => ParseEmpty<FinancialRuleCategory>();

    static public FixedList<FinancialRuleCategory> GetList() {
      return GetStorageObjects<FinancialRuleCategory>();
    }

    #endregion Constructors and parsers

    public new string NamedKey {
      get {
        return base.NamedKey;
      }
    }

  } // class FinancialRuleCategory

} // namespace Empiria.Financial.Rules
