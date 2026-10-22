"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

export default function TeamsPage() {
  const [teams, setTeams] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [name, setName] = useState("");
  const [message, setMessage] = useState("");

  const loadTeams = async () => {
    try {
      const response = await fetch(
        `https://localhost:7103/api/teams?pageNumber=${pageNumber}&pageSize=2&name=${name}&sortBy=Id&sortDirection=asc`
      );

      if (!response.ok) {
        throw new Error();
      }

      const result = await response.json();

      setTeams(result.data);
      setTotalPages(result.totalPages);
    } catch {
      setMessage("Error al cargar los equipos.");
    }
  };

  useEffect(() => {
    loadTeams();
  }, [pageNumber, name]);

  const deleteTeam = async (id) => {
    const confirmed = window.confirm(
      "¿Está seguro de eliminar este equipo?"
    );

    if (!confirmed) {
      return;
    }

    try {
      const response = await fetch(
        `https://localhost:7103/api/teams/${id}`,
        {
          method: "DELETE",
        }
      );

      if (!response.ok) {
        setMessage(
          "No se pudo eliminar el equipo."
        );
        return;
      }

      setMessage("Equipo eliminado correctamente.");

      await loadTeams();
    } catch {
      setMessage("Error al conectar con la API.");
    }
  };

  return (
    <main>
      <div className="pageHeader">
        <h1>Equipos</h1>

        <Link href="/teams/create" className="addButton">
          + Agregar equipo
        </Link>
      </div>

      {message && <p>{message}</p>}

      <div className="searchBox">
        <label>Buscar equipo:</label>

        <input
          type="text"
          value={name}
          onChange={(e) => {
            setName(e.target.value);
            setPageNumber(1);
          }}
          placeholder="Nombre del equipo"
        />
      </div>

      <table>
        <thead>
          <tr>
            <th>Id</th>
            <th>Equipo</th>
            <th>Acciones</th>
          </tr>
        </thead>

        <tbody>
          {teams.map((team) => (
            <tr key={team.id}>
              <td>{team.id}</td>

              <td>{team.name}</td>

              <td className="actions">
                <Link
                  href={`/players?teamId=${team.id}`}
                  className="viewButton"
                >
                  Integrantes
                </Link>

       <Link
  href={`/teams/edit?id=${team.id}&name=${encodeURIComponent(team.name)}`}
  className="editButton"
>
  Editar
</Link>

                <button
                  className="deleteButton"
                  onClick={() => deleteTeam(team.id)}
                >
                  Eliminar
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className="pagination">
        <button
          className="pageButton"
          onClick={() => setPageNumber(pageNumber - 1)}
          disabled={pageNumber === 1}
        >
          ‹
        </button>

        <span>
          Página {pageNumber} de {totalPages}
        </span>

        <button
          className="pageButton"
          onClick={() => setPageNumber(pageNumber + 1)}
          disabled={pageNumber === totalPages}
        >
          ›
        </button>
      </div>

      <Link href="/" className="backButton">
        ← Volver al inicio
      </Link>
    </main>
  );
}