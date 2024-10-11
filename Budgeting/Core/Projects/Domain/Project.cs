/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Project                                    Component : Domain Layer                            *
*  Assembly : Empiria.Projects.Core.dll                  Pattern   : Partitioned Type + Aggregate Root       *
*  Type     : Budget                                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents a project and serves as an aggregate root for its tasks.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Budgeting;

using Empiria.Projects.Adapters;

namespace Empiria.Projects {

  /// <summary>Partitioned type that represents a proyect and serves as an aggregate root
  /// for its tasks.</summary>
  public class Project : BudgetSegmentItem {

    #region Constructors and parsers

    public Project(ProjectFields fields) : base(BudgetSegmentType.ProjectType) {
      Assertion.Require(fields, nameof(fields));

      Load(fields);
    }


    private Project(BudgetSegmentType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    public Project(ProjectType projectType) : base(BudgetSegmentType.ProjectType) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new Project Parse(int id) {
      return BaseObject.ParseId<Project>(id);
    }

    static public new Project Parse(string typeUID) => ParseKey<Project>(typeUID);

    static public FixedList<Project> GetList() {
      return BaseObject.GetList<Project>(string.Empty, string.Empty)
                       .FindAll(x => x.Status == StateEnums.EntityStatus.Active)
                       .ToFixedList();
    }


    static public FixedList<Project> GetList(ProjectType projectType) {
      return BaseObject.GetList<Project>(string.Empty, string.Empty)
                       .FindAll(x => x.Status == StateEnums.EntityStatus.Active)
                       .ToFixedList();
    }

    static public new Project Empty => BaseObject.ParseEmpty<Project>();

    #endregion Constructors and parsers

    private void Load(ProjectFields fields) {

      base.Name = fields.Name;
      base.Description = fields.Description;
      base.Code = fields.Code;
    }

  }  // class Project

}  // namespace Empiria.Projects
