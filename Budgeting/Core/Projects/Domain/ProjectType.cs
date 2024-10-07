/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : ProjectType                                Component : Domain Layer                            *
*  Assembly : Empiria.Projects.dll                       Pattern   : Power Type                              *
*  Type     : Projects                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Type that describes a project type.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Security.AccessControl;
using Empiria.Json;
using Empiria.Projects.Adapters;
using Empiria.StateEnums;


namespace Empiria.ProjectTypes {

  /// <summary>Type that describes a project type.</summary>

  public class ProjectType : GeneralObject {

    #region Constructors and parsers

    private ProjectType() {
      // Empiria power types always have this constructor.
    }

    public ProjectType(ProjectTypeFields fields) {
      Assertion.Require(fields, nameof(fields));

      Load(fields);
    }


    static public ProjectType Parse(int typeId) => ParseId<ProjectType>(typeId);

    static public ProjectType Parse(string typeUID) => ParseKey<ProjectType>(typeUID);

    static public FixedList<ProjectType> GetList() {
      return BaseObject.GetList<ProjectType>(string.Empty)
                       .FindAll(x => x.Status != StateEnums.EntityStatus.Active)
                       .ToFixedList();

    }

    static public ProjectType Empty => ParseEmpty<ProjectType>();

    #endregion Constructors and parsers

    #region Properties

    public string Code {
      get {
        return base.ExtendedDataField.Get<string>("code", string.Empty);
      }
      set {
        base.ExtendedDataField.SetIfValue("code", value);
      }
    }

    #endregion Properties

    #region Helpers

    private void Load(ProjectTypeFields fields) {
      base.Name = fields.Name;
      Code = fields.Code;

    }

    internal void Update(ProjectTypeFields fields) {
      Load(fields);
    }

    internal void Delete() {
      base.Status = StateEnums.EntityStatus.Deleted;
    }

    #endregion Helpers


  }  // class ProjectType

}  // Empiria.Projects
