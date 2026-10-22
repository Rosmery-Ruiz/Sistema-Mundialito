import Link from "next/link";

export default function Home() {
  return (
    <main>
      <h1>Mundialito de Fútbol</h1>
      <p>Seleccione una opción:</p>

      <nav>
        <ul>
          <li>
            <Link href="/teams">Equipos</Link>
          </li>

          <li>
            <Link href="/matches">Partidos y registro de resultados</Link>
          </li>

          <li>
            <Link href="/standings">Tabla de posiciones</Link>
          </li>

          <li>
            <Link href="/scorers">Goleadores</Link>
          </li>
        </ul>
      </nav>
    </main>
  );
}