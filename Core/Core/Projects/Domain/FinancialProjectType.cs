/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Power type                              *
*  Type     : FinancialProjectType                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a financial project.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Financial.Projects {

  /// <summary>Power type that describes a financial project.</summary>
  [Powertype(typeof(FinancialProject))]
  public sealed class FinancialProjectType : Powertype {

    #region Constructors and parsers

    private FinancialProjectType() {
      // Empiria power types always have this constructor.
    }

    static public new FinancialProjectType Parse(int typeId) => Parse<FinancialProjectType>(typeId);

    static public new FinancialProjectType Parse(string typeName) => Parse<FinancialProjectType>(typeName);

    static public FixedList<FinancialProjectType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (FinancialProjectType) x)
            .ToFixedList();
    }

    static public FinancialProjectType Empty => Parse("ObjectTypeInfo.FinancialProject");

    #endregion Constructors and parsers

    #region Properties

    public bool IsProtected {
      get {
        return ExtensionData.Get("isProtected", false);
      }
    }

    #endregion Properties

  } // class FinancialProjectType

}  // namespace Empiria.Financial.Projects
