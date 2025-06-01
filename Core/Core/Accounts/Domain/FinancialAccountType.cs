/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Power type                              *
*  Type     : FinancialAccountType                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a financial account.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Financial {

  /// <summary>Power type that describes a financial account.</summary>
  [Powertype(typeof(FinancialAccount))]
  public sealed class FinancialAccountType : Powertype {

    #region Constructors and parsers

    private FinancialAccountType() {
      // Empiria power types always have this constructor.
    }

    static public new FinancialAccountType Parse(int typeId) => Parse<FinancialAccountType>(typeId);

    static public new FinancialAccountType Parse(string typeName) => Parse<FinancialAccountType>(typeName);

    static public FixedList<FinancialAccountType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (FinancialAccountType) x)
            .ToFixedList();
    }

    static public FinancialAccountType Empty => Parse("ObjectTypeInfo.FinancialAccount");

    #endregion Constructors and parsers

    #region Properties

    public FixedList<string> Roles {
      get {
        return base.ExtensionData.GetFixedList<string>("roles", false);
      }
    }

    #endregion Properties

    #region Methods

    public bool PlaysRole(string role) {
      Assertion.Require(role, nameof(role));

      return Roles.Contains(role);
    }

    #endregion Methods

  } // class FinancialAccountType

}  // namespace Empiria.Financial
