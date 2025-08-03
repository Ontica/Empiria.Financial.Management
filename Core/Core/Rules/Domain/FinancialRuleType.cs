/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Rules                            Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Power type                              *
*  Type     : FinancialRuleType                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a financial rule.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Financial.Rules {

  /// <summary>Power type that describes a financial rule.</summary>
  [Powertype(typeof(FinancialRule))]
  public sealed class FinancialRuleType : Powertype {

    #region Constructors and parsers

    private FinancialRuleType() {
      // Empiria power types always have this constructor.
    }

    static public new FinancialRuleType Parse(int typeId) => Parse<FinancialRuleType>(typeId);

    static public new FinancialRuleType Parse(string typeName) => Parse<FinancialRuleType>(typeName);

    static public FixedList<FinancialRuleType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (FinancialRuleType) x)
            .ToFixedList();
    }

    static public FinancialRuleType Empty => Parse("ObjectTypeInfo.FinancialRule");

    #endregion Constructors and parsers

  } // class FinancialRuleType

}  // namespace Empiria.Financial.Rules
