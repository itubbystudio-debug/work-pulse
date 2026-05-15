<script setup lang="ts">
const architectureCards = [
  {
    title: 'Backend Foundation',
    summary: '.NET 10 Web API split into Domain, Application, Infrastructure, and WebApi layers.',
    icon: 'pi pi-server'
  },
  {
    title: 'CQRS Workflow',
    summary: 'MediatR drives one starter command and one starter query to establish the request pipeline.',
    icon: 'pi pi-send'
  },
  {
    title: 'Data Access',
    summary: 'EF Core targets SQL Server as the default ORM while Dapper remains ready for raw SQL workloads.',
    icon: 'pi pi-database'
  }
];
</script>

<template>
  <div class="app-shell">
    <aside class="sidebar">
      <div class="brand">
        <span class="brand-mark">WP</span>
        <div>
          <p class="eyebrow">Sakai Base</p>
          <h1>Work Pulse</h1>
        </div>
      </div>

      <nav class="nav-list">
        <a href="#overview">Overview</a>
        <a href="#stack">Stack</a>
        <a href="#workflow">Workflow</a>
      </nav>

      <div class="sidebar-card">
        <span class="caption">Frontend stack</span>
        <strong>Vue 3 + PrimeVue</strong>
        <p>Styled as a Sakai-inspired admin landing shell for future modules.</p>
      </div>
    </aside>

    <main class="content">
      <section id="overview" class="hero">
        <p-tag severity="contrast" value="Project Scaffold" rounded></p-tag>
        <h2>Clean Standard baseline for a new .NET 10 and Vue workspace.</h2>
        <p>
          This starter keeps the backend and frontend deliberately small while preserving the architecture,
          dependencies, and conventions the team asked for.
        </p>

        <div class="hero-actions">
          <p-button label="Inspect API Layer" icon="pi pi-arrow-right"></p-button>
          <p-button label="Review Theme Shell" icon="pi pi-palette" severity="secondary" outlined></p-button>
        </div>
      </section>

      <section id="stack" class="grid">
        <p-card v-for="card in architectureCards" :key="card.title">
          <template #title>
            <span class="card-title"><i :class="card.icon"></i>{{ card.title }}</span>
          </template>
          <template #content>
            <p>{{ card.summary }}</p>
          </template>
        </p-card>
      </section>

      <p-divider></p-divider>

      <section id="workflow" class="workflow">
        <div>
          <span class="caption">Workflow</span>
          <h3>Scaffolded to let the team start shipping feature slices immediately.</h3>
        </div>
        <ul>
          <li>Commands and queries are separated through MediatR handlers.</li>
          <li>Infrastructure exposes SQL Server, EF Core, and Dapper registration.</li>
          <li>PrimeVue components and a Sakai-style shell are ready for expansion.</li>
        </ul>
      </section>
    </main>
  </div>
</template>
