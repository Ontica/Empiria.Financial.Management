/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Project Types Management                   Component : Adapters Layer                          *
*  Assembly : Empiria.Projects.Core.dll                  Pattern   : Fields DTO                              *
*  Type     : ProjectTypesFields                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO fields structure used for update project types information.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Xml.Linq;

namespace Empiria.Projects.Adapters {

  /// <summary>DTO fields structure used for update project types information.</summary>
  public class ProjectTypeFields {


    public string ID {
      get; set;
    } = string.Empty;

    public string UID {
      get; set;
    } = string.Empty;

    public string Name {
      get; set;
    } = string.Empty;

    public string Code {
      get; set;
    } = string.Empty;

    internal void EnsureValid() {
      Assertion.Require(Name, "Necesito el nombre del tipo de proyecto.");
    }

    }  // class ProjectTypeFields

  }  // namespace Empiria.Projects.Adapters
