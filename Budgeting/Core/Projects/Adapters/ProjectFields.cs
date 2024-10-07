/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Project Management                         Component : Adapters Layer                          *
*  Assembly : Empiria.Projects.Core.dll                  Pattern   : Fields DTO                              *
*  Type     : ProjectFields                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO fields structure used for update projects information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Projects.Adapters {

  /// <summary>DTO fields structure used for update project types information.</summary>
  public class ProjectFields {

    public string ProjectTypeUID {
      get; set;
    }

    public string Name {
      get; set;
    }


    public string Code {
      get; set;
    }


    public string Description {
      get; set;
    }

    internal void EnsureValid() {
      Assertion.Require(Name, "Necesito el nombre del proyecto.");
    }

  }  // class ProjectFields

}  // namespace Empiria.Projects.Adapters