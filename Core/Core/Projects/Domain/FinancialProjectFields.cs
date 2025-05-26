/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Domain Layer                          *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input fields                          *
*  Type     : FinancialProjectFields                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields with financial projects data.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Financial.Projects {

  /// <summary>Input fields with financial projects data.</summary>
  public class FinancialProjectFields {

    #region Properties

    public string CategoryUID {
      get; set;
    } = string.Empty;


    public string PartyUID {
      get; set;
    } = string.Empty;


    public string SubprogramUID {
      get; set;
    } = string.Empty;


    public string ProjectNo {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    #endregion Properties

    #region Methods

    internal void EnsureValid() {
      ProjectNo = FieldPatcher.Clean(ProjectNo);
      Name = FieldPatcher.Clean(Name);

      CategoryUID = FieldPatcher.Clean(CategoryUID);
      SubprogramUID = FieldPatcher.Clean(SubprogramUID);
      PartyUID = FieldPatcher.Clean(PartyUID);

      if (CategoryUID.Length != 0) {
        _ = FinancialProjectCategory.Parse(CategoryUID);
      }

      if (SubprogramUID.Length != 0) {
        _ = StandardAccount.Parse(SubprogramUID);
      }

      if (PartyUID.Length != 0) {
        _ = Party.Parse(PartyUID);
      }
    }

    #endregion Methods

  }  // class FinancialProjectFields

}  // namespace Empiria.Financial.Projects
