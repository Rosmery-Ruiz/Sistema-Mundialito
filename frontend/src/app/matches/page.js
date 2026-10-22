"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

export default function MatchesPage() {
  const [matches, setMatches] = useState([]);
  const [error, setError] = useState("");

const [matchId, setMatchId] = useState("");
const [homeScore, setHomeScore] = useState("");
const [awayScore, setAwayScore] = useState("");
const [homeScorers, setHomeScorers] = useState("");
const [awayScorers, setAwayScorers] = useState("");
const [message, setMessage] = useState("");
const [pageNumber, setPageNumber] = useState(1);
const [totalPages, setTotalPages] = useState(1);

const [teamId, setTeamId] = useState("");
const [status, setStatus] = useState("");
const [matchDate, setMatchDate] = useState("");
const [teams, setTeams] = useState([]);

  const loadMatches = () => {
    fetch(
  `https://localhost:7103/api/matches?pageNumber=${pageNumber}&pageSize=2&matchDate=${matchDate}&teamId=${teamId}&status=${status}&sortBy=MatchDate&sortDirection=asc`
)
      .then((response) => {
        if (!response.ok) {
          throw new Error("No se pudieron cargar los partidos.");
        }

        return response.json();
      })
      .then((result) => {
  setMatches(result.data);
  setTotalPages(result.totalPages);
  setError("");
})
      .catch(() => {
        setError("Error al cargar los partidos.");
      });
  };

 useEffect(() => {
  loadMatches();
}, [pageNumber, teamId, status, matchDate]);

useEffect(() => {
  fetch(
    "https://localhost:7103/api/teams?pageNumber=1&pageSize=100&sortBy=Name&sortDirection=asc"
  )
    .then((response) => response.json())
    .then((result) => setTeams(result.data))
    .catch(() => setError("Error al cargar los equipos."));
}, []);

const registerResult = async (e) => {
  e.preventDefault();

  try {
    const response = await fetch(
      `https://localhost:7103/api/matches/${matchId}/result`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          matchId: Number(matchId),
          homeScore: Number(homeScore),
          awayScore: Number(awayScore),
          homeScorerIds: homeScorers
            ? homeScorers.split(",").map(Number)
            : [],
          awayScorerIds: awayScorers
            ? awayScorers.split(",").map(Number)
            : [],
        }),
      }
    );

    if (!response.ok) {
      throw new Error();
    }

    setMessage("Resultado registrado correctamente.");
    setError("");

    loadMatches();
  } catch {
    setError("No se pudo registrar el resultado.");
    setMessage("");
  }
};

  return (
    <main>
      <div className="pageHeader">
  <h1>Partidos</h1>

  <Link href="/matches/create" className="addButton">
    + Agregar partido
  </Link>
</div>
      <h2>Registrar resultado</h2>

<form onSubmit={registerResult}>
  <div>
    <label>Id del partido: </label>
    <input
      type="number"
      value={matchId}
      onChange={(e) => setMatchId(e.target.value)}
      required
    />
  </div>

  <div>
    <label>Goles local: </label>
    <input
      type="number"
      min="0"
      value={homeScore}
      onChange={(e) => setHomeScore(e.target.value)}
      required
    />
  </div>

  <div>
    <label>Goles visitante: </label>
    <input
      type="number"
      min="0"
      value={awayScore}
      onChange={(e) => setAwayScore(e.target.value)}
      required
    />
  </div>

  <div>
    <label>IDs goleadores local: </label>
    <input
      type="text"
      value={homeScorers}
      onChange={(e) => setHomeScorers(e.target.value)}
      placeholder="Ej: 1,2"
    />
  </div>

  <div>
    <label>IDs goleadores visitante: </label>
    <input
      type="text"
      value={awayScorers}
      onChange={(e) => setAwayScorers(e.target.value)}
      placeholder="Ej: 6"
    />
  </div>

  <button type="submit">Registrar resultado</button>
</form>


<div>
  <label>Equipo: </label>
 <select
  value={teamId}
  onChange={(e) => {
    setTeamId(e.target.value);
    setPageNumber(1);
  }}
>
  <option value="">Todos</option>

  {teams.map((team) => (
    <option key={team.id} value={team.id}>
      {team.name}
    </option>
  ))}
</select>

  <label> Estado: </label>
  <select
    value={status}
    onChange={(e) => {
      setStatus(e.target.value);
      setPageNumber(1);
    }}
  >
    <option value="">Todos</option>
    <option value="Scheduled">Scheduled</option>
    <option value="Finished">Finished</option>
  </select>

  <label> Fecha: </label>
  <input
    type="date"
    value={matchDate}
    onChange={(e) => {
      setMatchDate(e.target.value);
      setPageNumber(1);
    }}
  />
</div>

{message && <p>{message}</p>}

      {error && <p>{error}</p>}

      <table border="1" cellPadding="8">
        <thead>
          <tr>
            <th>Id</th>
            <th>Local</th>
            <th>Visitante</th>
            <th>Fecha</th>
            <th>Resultado</th>
            <th>Estado</th>
          </tr>
        </thead>

        <tbody>
          {matches.map((match) => (
            <tr key={match.id}>
              <td>{match.id}</td>
              <td>
  {teams.find((team) => team.id === match.homeTeamId)?.name}
</td>

<td>
  {teams.find((team) => team.id === match.awayTeamId)?.name}
</td>
              <td>{match.matchDate}</td>
              <td>
                {match.homeScore ?? "-"} - {match.awayScore ?? "-"}
              </td>
              <td>{match.status}</td>
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