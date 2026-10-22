"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

export default function ScorersPage() {
  const [scorers, setScorers] = useState([]);
  const [error, setError] = useState("");
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    fetch(
     `https://localhost:7103/api/scorers?pageNumber=${pageNumber}&pageSize=3&sortBy=Goals&sortDirection=desc`
    )
      .then((response) => {
        if (!response.ok) {
          throw new Error();
        }

        return response.json();
      })
      .then((result) => {
  setScorers(result.data);
  setTotalPages(result.totalPages);
  setError("");
})
      .catch(() => {
        setError("Error al cargar los goleadores.");
      });
 }, [pageNumber]);

  return (
    <main>
      <h1>Goleadores</h1>

      {error && <p>{error}</p>}

      <table border="1" cellPadding="8">
        <thead>
          <tr>
            <th>Jugador</th>
            <th>Equipo</th>
            <th>Goles</th>
          </tr>
        </thead>

        <tbody>
          {scorers.map((player) => (
            <tr key={player.playerId}>
              <td>{player.playerName}</td>
              <td>{player.teamName}</td>
              <td>{player.goals}</td>
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