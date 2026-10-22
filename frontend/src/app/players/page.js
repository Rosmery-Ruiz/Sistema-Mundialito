"use client";

import { useEffect, useState } from "react";
import { useSearchParams } from "next/navigation";
import Link from "next/link";

export default function PlayersPage() {
  const searchParams = useSearchParams();

  const teamId = searchParams.get("teamId");

  const [players, setPlayers] = useState([]);
  const [teams, setTeams] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [message, setMessage] = useState("");

  useEffect(() => {
    const loadTeams = async () => {
      try {
        const response = await fetch(
          "https://localhost:7103/api/teams?pageNumber=1&pageSize=100&sortBy=Id&sortDirection=asc"
        );

        if (!response.ok) {
          throw new Error();
        }

        const result = await response.json();

        setTeams(result.data);
      } catch {
        setMessage("Error al cargar el equipo.");
      }
    };

    loadTeams();
  }, []);

  useEffect(() => {
    const loadPlayers = async () => {
      try {
        let url =
          `https://localhost:7103/api/players?pageNumber=${pageNumber}&pageSize=5&sortBy=Id&sortDirection=asc`;

        if (teamId) {
          url += `&teamId=${teamId}`;
        }

        const response = await fetch(url);

        if (!response.ok) {
          throw new Error();
        }

        const result = await response.json();

        setPlayers(result.data);
        setTotalPages(result.totalPages);
      } catch {
        setMessage("Error al cargar los integrantes.");
      }
    };

    loadPlayers();
  }, [teamId, pageNumber]);

  const selectedTeam = teams.find(
    (team) => team.id === Number(teamId)
  );

  return (
    <main>
      <div className="pageHeader">
        <h1>
          {selectedTeam
            ? `Integrantes - ${selectedTeam.name}`
            : "Integrantes de equipos"}
        </h1>

        {teamId && (
          <Link
            href={`/players/create?teamId=${teamId}`}
            className="addButton"
          >
            + Agregar integrante
          </Link>
        )}
      </div>

      {message && <p>{message}</p>}

      <table>
        <thead>
          <tr>
            <th>Id</th>
            <th>Nombre</th>
            <th>Número</th>
          </tr>
        </thead>

        <tbody>
          {players.map((player) => (
            <tr key={player.id}>
              <td>{player.id}</td>
              <td>{player.name}</td>
              <td>{player.shirtNumber}</td>
            </tr>
          ))}
        </tbody>
      </table>

      {players.length === 0 && (
        <p>No hay integrantes registrados.</p>
      )}

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
          disabled={
            pageNumber === totalPages ||
            totalPages === 0
          }
        >
          ›
        </button>
      </div>

      <Link href="/teams" className="backButton">
        ← Volver a equipos
      </Link>
    </main>
  );
}