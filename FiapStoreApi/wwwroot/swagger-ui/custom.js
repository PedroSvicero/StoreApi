// Customizacao visual e didatica do Swagger UI.
// Este script injeta um cabecalho com orientacoes rapidas sem alterar a logica da API.

window.addEventListener("load", () => {
    const swaggerRoot = document.querySelector(".swagger-ui");
    if (!swaggerRoot) {
        return;
    }

    customizeTopbar(swaggerRoot);
    injectHero(swaggerRoot);
});

function customizeTopbar(swaggerRoot) {
    const topbarWrapper = swaggerRoot.querySelector(".topbar-wrapper");
    if (!topbarWrapper || topbarWrapper.querySelector(".fiap-brand")) {
        return;
    }

    const brand = document.createElement("div");
    brand.className = "fiap-brand";
    brand.innerHTML = `
        <span class="fiap-brand__title">FiapStoreApi</span>
        <span class="fiap-brand__subtitle">Documentacao interativa para estudo de API, JWT, EF Core e Dapper</span>
    `;

    topbarWrapper.prepend(brand);
}

function injectHero(swaggerRoot) {
    if (document.querySelector(".fiap-hero")) {
        return;
    }

    const infoContainer = swaggerRoot.querySelector(".information-container.wrapper");
    if (!infoContainer || !infoContainer.parentElement) {
        return;
    }

    const hero = document.createElement("section");
    hero.className = "fiap-hero";
    hero.innerHTML = `
        <div class="fiap-hero__panel">
            <div class="fiap-hero__content">
                <div>
                    <h1>Guia rapido para navegar e testar a API</h1>
                    <p>
                        Esta tela foi montada para funcionar como documentacao e laboratorio.
                        Leia os resumos dos endpoints, veja os exemplos e use o botao <code>Authorize</code>
                        para testar os fluxos protegidos.
                    </p>
                </div>
                <div class="fiap-hero__notes">
                    <div class="fiap-card">
                        <strong>Fluxo sugerido</strong>
                        1. Registrar usuario. 2. Fazer login. 3. Autorizar com Bearer token. 4. Explorar clientes, livros e pedidos.
                    </div>
                    <div class="fiap-card">
                        <strong>Leitura didatica</strong>
                        Os endpoints de leitura em livros e pedidos usam Dapper; as operacoes de escrita continuam em EF Core.
                    </div>
                </div>
            </div>
        </div>
    `;

    infoContainer.parentElement.insertBefore(hero, infoContainer);
}
