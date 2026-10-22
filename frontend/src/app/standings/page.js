"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

export default function StandingsPage() {
  const [standings, setStandings] = useState([]);
  const [error, setError] = useState("");
  const [pageNumber, setPageNumber] = useState(1);
const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    fetch(
      `https://localhost:7103/api/standings?pageNumber=${pageNumber}&pageSize=2&sortBy=Points&sortDirection=desc`
    )
      .then((response) => {
        if (!response.ok) {
          throw new Error();
        }

        return response.json();
      })
      .then((result) => {
        setStandings(result.data);
        setTotalPages(result.totalPages);
        setError("");
      })
      .catch(() => {
        setError("Error al cargar la tabla de posiciones.");
      });
  }, [pageNumber]);

  return (
    <main>
      <h1>Tabla de posiciones</h1>
    
      {error && <p>{error}</p>}

      <table border="1" cellPadding="8">
        <thead>
          <tr>
            <th>Equipo</th>
            <th>PJ</th>
            <th>PG</th>
            <th>PE</th>
            <th>PP</th>
            <th>GF</th>
            <th>GC</th>
            <th>DG</th>
            <th>Pts</th>
          </tr>
        </thead>

        <tbody>
          {standings.map((team) => (
            <tr key={team.teamId}>
              <td>{team.teamName}</td>
              <td>{team.played}</td>
              <td>{team.won}</td>
              <td>{team.drawn}</td>
              <td>{team.lost}</td>
              <td>{team.goalsFor}</td>
              <td>{team.goalsAgainst}</td>
              <td>{team.goalDifference}</td>
              <td>{team.points}</td>
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