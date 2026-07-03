# Maxsys.Bootstrap

Componentes **Bootstrap 5.3** para *ASP.NET Core MVC* (.NET 10): **TagHelpers** (`<bs-*>`, conteúdo projetado) e **ViewComponents** (`<vc:bs-*>`, data-driven).

## Índice

* [Setup](#setup)
* [Customização global (Defaults)](#customização-global-defaults)
* [TagHelpers](#taghelpers)
* [ViewComponents (data-driven)](#viewcomponents-data-driven)

## Setup

**TagHelpers** (`<bs-*>`): basta registrar o assembly no `_ViewImports.cshtml`:

```cshtml
@addTagHelper *, Maxsys.Bootstrap
```

**ViewComponents** (`<vc:bs-*>`): requerem o registro do assembly como *Application Part* do MVC, via `AddMaxsysBootstrap()`:

```csharp
// Program.cs
builder.Services.AddControllersWithViews().AddMaxsysBootstrap();
```

> :warning: A biblioteca emite **apenas markup** (classes CSS e atributos `data-bs-*`). O Bootstrap 5.3 (CSS/JS) e o Bootstrap Icons **não** são incluídos — referenciá-los (CDN ou bundle local) é responsabilidade da aplicação.

## Customização global (Defaults)

Cada componente possui uma classe estática `XDefaults` (`ButtonDefaults`, `AlertDefaults`, `ModalDefaults`...) com os valores padrão de todos os seus atributos. A aplicação pode alterar esses defaults **em um único lugar** (tipicamente no startup), afetando todas as ocorrências do componente que não definirem o atributo explicitamente:

```csharp
// Program.cs — muda o padrão de todos os <bs-button> e <bs-alert> da aplicação
ButtonDefaults.Variant = ButtonVariants.Secondary;
ButtonDefaults.Size = ButtonSizes.Small;
AlertDefaults.Icon = BootstrapIcons.InfoCircleFill;
ModalDefaults.IsCentered = true;
```

Os ViewComponents seguem o mesmo padrão (`BsPaginationViewDefaults`, `BsBreadcrumbViewDefaults`, `BsCarouselViewDefaults`).

### Traits (atributos compartilhados)

Vários TagHelpers implementam *traits* (interfaces com implementação padrão) que adicionam atributos comuns:

* **`IBootstrapText`** (texto) — disponível nos componentes marcados com *[texto]* abaixo:
    * `text-transform:TextTransformations` (`LowerCase`/`UpperCase`/`Capitalize` → `text-lowercase`...)
    * `font-weight:FontWeights` (`Bold`/`Bolder`/`Semibold`/`Medium`/`Normal`/`Light`/`Lighter` → `fw-*`)
    * `text-size:FontSizes` (`Size1`..`Size6` → `fs-1`..`fs-6`)
    * `text-color:TextColors` (`Primary`, `PrimaryEmphasis`, ..., `Black50`, `White50` → `text-*`)
    * `custom-fg:string` — cor CSS livre (ex.: `#ff8800`, `rebeccapurple`); tem precedência sobre `text-color`
    * `italic:bool` (`fst-italic`), `monospace:bool` (`font-monospace`) e, em alguns componentes, `small:bool`
* **`IBootstrapBackground`** (fundo) — disponível nos componentes marcados com *[fundo]*:
    * `background-color:BackgroundColors` (`Primary`, `PrimarySubtle`, ..., `Transparent` → `bg-*`)
    * `custom-bg:string` — cor CSS livre; tem precedência sobre `background-color`

Exceções pontuais (verificadas no código): em `bs-alert`, `bs-badge` e `bs-button` o `background-color` por enum não é bindável (use `custom-bg`); `bs-badge` só expõe `size`, `custom-fg`, `small`, `italic` e `monospace` do trait de texto; `bs-breadcrumb`, `bs-breadcrumb-item` e `bs-label` usam `custom-color` no lugar de `custom-fg`; nos componentes de tabela, cores custom são aplicadas via CSS vars (`--bs-table-color`/`--bs-table-bg`).

## TagHelpers

### `<bs-accordion>` (Accordion)

Acordeão com painéis colapsáveis. `id` é gerado automaticamente quando não informado; os alvos de collapse dos itens são amarrados sozinhos.

| Tag | Atributos |
|---|---|
| `bs-accordion` | `flush:bool` (accordion-flush), `always-open:bool` (painéis independentes, sem `data-bs-parent`) |
| `bs-accordion-item` | — |
| `bs-accordion-header` | `class`/`style` (repassados ao botão interno) |
| `bs-accordion-body` | `class`/`style` (repassados ao corpo interno) |

```html
<bs-accordion always-open="true">
    <bs-accordion-item>
        <bs-accordion-header>Item #1</bs-accordion-header>
        <bs-accordion-body>Conteúdo do primeiro painel.</bs-accordion-body>
    </bs-accordion-item>
    <bs-accordion-item>
        <bs-accordion-header>Item #2</bs-accordion-header>
        <bs-accordion-body>Conteúdo do segundo painel.</bs-accordion-body>
    </bs-accordion-item>
</bs-accordion>
```

### `<bs-alert>` (Alert)

Alerta contextual (`div.alert`, `role="alert"`), com ícone opcional e cabeçalho. *[texto]* *[fundo: só `custom-bg`]*

| Tag | Atributos |
|---|---|
| `bs-alert` | `type:AlertTypes` (`Primary`..`Dark`, `Body`, `BodySecondary`, `BodyTertiary`, `Black`, `White`), `icon:BootstrapIcons`, `small:bool` + trait de texto |
| `bs-alert-header` | renderiza `h4.alert-heading` + trait de texto |

```html
<bs-alert type="Warning" icon="ExclamationTriangleFill">
    <bs-alert-header>Atenção</bs-alert-header>
    Verifique os dados antes de salvar.
</bs-alert>
```

### `<bs-badge>` (Badge)

Emblema (`span.badge`) com variantes normais e *subtle*. *[texto: parcial]* *[fundo: só `custom-bg`]*

| Tag | Atributos |
|---|---|
| `bs-badge` | `type:BadgeTypes` (`Primary`, `PrimarySubtle`, ..., `Black`, `White`), `rounded:bool` (rounded-pill), `size:FontSizes`, `custom-fg`, `custom-bg`, `small:bool`, `italic:bool`, `monospace:bool` |

```html
<h4>Notificações <bs-badge type="DangerSubtle" rounded="true">4</bs-badge></h4>
```

### `<bs-breadcrumb>` (Breadcrumb)

Trilha de navegação (`nav > ol.breadcrumb`). *[texto, com `custom-color`]*

| Tag | Atributos |
|---|---|
| `bs-breadcrumb` | `divider:string` (CSS var `--bs-breadcrumb-divider`; padrão `/`) + trait de texto (`custom-color`, `small`) |
| `bs-breadcrumb-item` | `active:bool` + trait de texto (`custom-color`, `small`) |

```html
<bs-breadcrumb divider=">">
    <bs-breadcrumb-item><a href="/">Home</a></bs-breadcrumb-item>
    <bs-breadcrumb-item><a href="/produtos">Produtos</a></bs-breadcrumb-item>
    <bs-breadcrumb-item active="true">Detalhes</bs-breadcrumb-item>
</bs-breadcrumb>
```

> Para breadcrumb gerado a partir de um modelo, veja o ViewComponent [`vc:bs-breadcrumb`](#vcbs-breadcrumb).

### `<bs-button>` (Button)

Botão: renderiza `<button>` ou, quando `href` é informado, `<a role="button">`. *[texto]* *[fundo: só `custom-bg`]*

| Tag | Atributos |
|---|---|
| `bs-button` | `variant:ButtonVariants` (`Primary`..`Dark`, `Link`), `outline:bool` (btn-outline-*), `size:ButtonSizes` (`Small`/`Large`), `type:ButtonTypes` (`Button`/`Submit`/`Reset`; ignorado com `href`), `href:string`, `disabled:bool`, `icon:BootstrapIcons`, `no-wrap:bool` + trait de texto |

```html
<bs-button variant="Success" type="Submit" icon="CheckLg">Salvar</bs-button>
<bs-button variant="Secondary" outline="true" href="/voltar">Cancelar</bs-button>
```

### `<bs-button-group>` (ButtonGroup)

Grupo de botões (`div.btn-group`, `role="group"`) e toolbar (`div.btn-toolbar`, `role="toolbar"`).

| Tag | Atributos |
|---|---|
| `bs-button-group` | `size:ButtonGroupSizes` (`Small`/`Large`), `vertical:bool` (btn-group-vertical) |
| `bs-button-toolbar` | `gap:int` (0–5) |

```html
<bs-button-toolbar gap="2">
    <bs-button-group size="Small">
        <bs-button variant="Primary">1</bs-button>
        <bs-button variant="Primary">2</bs-button>
    </bs-button-group>
    <bs-button-group size="Small">
        <bs-button variant="Danger" icon="Trash">Excluir</bs-button>
    </bs-button-group>
</bs-button-toolbar>
```

### `<bs-card>` (Card)

Cartão com header/body/footer, título, texto, imagem e link. *[texto]* *[fundo]* (card, header e footer)

| Tag | Atributos |
|---|---|
| `bs-card` | `width:string` (ex.: `18rem`), `border-color:BorderColors` + traits de texto e fundo |
| `bs-card-header` / `bs-card-footer` | traits de texto e fundo |
| `bs-card-body` | — |
| `bs-card-title` (`h5.card-title`) / `bs-card-text` (`p.card-text`) | trait de texto |
| `bs-card-subtitle` | renderiza `h6.card-subtitle.mb-2.text-body-secondary` |
| `bs-card-img` | `position:CardImagePositions` (`Top`/`Bottom`), `src:string`, `alt:string` |
| `bs-card-link` | renderiza `a.card-link` (`href` é repassado) |

```html
<bs-card width="18rem" border-color="Primary">
    <bs-card-img position="Top" src="/img/capa.jpg" alt="Capa" />
    <bs-card-header background-color="PrimarySubtle">Destaque</bs-card-header>
    <bs-card-body>
        <bs-card-title>Título do card</bs-card-title>
        <bs-card-subtitle>Subtítulo</bs-card-subtitle>
        <bs-card-text>Um resumo rápido do conteúdo.</bs-card-text>
        <bs-card-link href="/detalhes">Ver mais</bs-card-link>
    </bs-card-body>
    <bs-card-footer>Atualizado há 3 min</bs-card-footer>
</bs-card>
```

### `<bs-carousel>` (Carousel)

Carrossel (`div.carousel.slide`) com controls e indicators opcionais. Como o pai é processado antes dos filhos, ao usar `indicators="true"` é obrigatório informar `slides` (quantidade).

| Tag | Atributos |
|---|---|
| `bs-carousel` | `fade:bool`, `autoplay:CarouselAutoplays` (`Carousel` inicia no load / `True` após interação), `interval:int?` (ms), `controls:bool`, `indicators:bool`, `slides:int` (obrigatório com indicators), `touch:bool` (padrão `true`), `keyboard:bool` (padrão `true`) |
| `bs-carousel-item` | `active:bool` (obrigatório em exatamente um slide), `interval:int?` (ms, individual) |
| `bs-carousel-caption` | renderiza `div.carousel-caption.d-none.d-md-block` |

```html
<bs-carousel controls="true" indicators="true" slides="2" autoplay="Carousel" interval="4000">
    <bs-carousel-item active="true">
        <img src="/img/1.jpg" class="d-block w-100" alt="Slide 1" />
        <bs-carousel-caption><h5>Primeiro</h5></bs-carousel-caption>
    </bs-carousel-item>
    <bs-carousel-item>
        <img src="/img/2.jpg" class="d-block w-100" alt="Slide 2" />
    </bs-carousel-item>
</bs-carousel>
```

> Para carrossel gerado a partir de um modelo (indicators contados automaticamente), veja o ViewComponent [`vc:bs-carousel`](#vcbs-carousel).

### `<bs-input-check>` / `<bs-input-switch>` (CheckInput/Switch)

Input de checkbox/switch integrado ao model binding (`asp-for`, obrigatório). Estende o `InputTagHelper` do ASP.NET Core e aplica `form-check-input`. Use dentro de [`bs-form-check`/`bs-form-switch`](#bs-form-floating--bs-form-input-group--bs-form-check--bs-form-switch-formgroup).

```html
<bs-form-switch>
    <bs-input-switch asp-for="Ativo" />
    <bs-form-check-label asp-for="Ativo" />
</bs-form-switch>
```

### `<bs-close-button>` (CloseButton)

Botão de fechar (`button.btn-close`) para dispensar modais, toasts, offcanvas e alerts.

| Tag | Atributos |
|---|---|
| `bs-close-button` | `white:bool` (btn-close-white), `disabled:bool`, `dismiss:CloseButtonDismissTargets` (`Modal`/`Toast`/`Offcanvas`/`Alert` → `data-bs-dismiss`), `label:string` (aria-label, padrão `Close`) |

```html
<bs-alert type="Info">
    Mensagem dispensável. <bs-close-button dismiss="Alert" />
</bs-alert>
```

### `<bs-collapse>` (Collapse)

Conteúdo colapsável (`div.collapse`) + gatilho. `id` é gerado quando ausente — informe um `id` para amarrar o trigger.

| Tag | Atributos |
|---|---|
| `bs-collapse` | `horizontal:bool` (collapse-horizontal; o filho precisa de largura definida), `show:bool` (inicia expandido) |
| `bs-collapse-trigger` | `target:string` (id do collapse, obrigatório — ou use `href`), `href:string` (renderiza `<a role="button">`), `color:CollapseTriggerColors` (`Primary`..`Dark`, `Link`), `outline:bool`, `size:CollapseTriggerSizes` (`Small`/`Large`), `expanded:bool` (use `true` quando o alvo tiver `show`) |

```html
<bs-collapse-trigger target="filtros" color="Secondary" outline="true">Filtros</bs-collapse-trigger>

<bs-collapse id="filtros">
    <div class="card card-body">Opções de filtro...</div>
</bs-collapse>
```

### `<bs-vr>` / `<bs-hr>` (Divider)

Divisores vertical (`div.vr`) e horizontal (`<hr>`).

| Tag | Atributos |
|---|---|
| `bs-vr` / `bs-hr` | `color:TextColors`, `custom-color:string` (cor CSS livre), `thickness:string` (ex.: `3px` — largura no `vr`, espessura da borda no `hr`) |

```html
<bs-hr color="Secondary" thickness="2px" />

<bs-hstack gap="3">
    <span>Esquerda</span>
    <bs-vr />
    <span>Direita</span>
</bs-hstack>
```

### `<bs-dropdown>` (Dropdown)

Menu suspenso completo: container, toggle, menu e itens.

| Tag | Atributos |
|---|---|
| `bs-dropdown` | `direction:DropdownDirections` (`Dropup`/`Dropend`/`Dropstart`/`DropupCenter`/`DropdownCenter`), `btn-group:bool` (para split buttons) |
| `bs-dropdown-toggle` | `color:DropdownToggleColors` (`Primary`..`Dark`, `Link`; padrão `Secondary`), `outline:bool`, `size:DropdownToggleSizes` (`Small`/`Large`), `split:bool` (dropdown-toggle-split) |
| `bs-dropdown-menu` | `dark:bool` (`data-bs-theme="dark"`), `end:bool` (dropdown-menu-end) |
| `bs-dropdown-item` | `href:string` (sem href renderiza `<button>`), `active:bool`, `disabled:bool` |
| `bs-dropdown-divider` | renderiza `li > hr.dropdown-divider` (tag sem conteúdo) |
| `bs-dropdown-header` | renderiza `li > h6.dropdown-header` |
| `bs-dropdown-text` | renderiza `li > span.dropdown-item-text` |

```html
<bs-dropdown>
    <bs-dropdown-toggle color="Primary">Ações</bs-dropdown-toggle>
    <bs-dropdown-menu>
        <bs-dropdown-header>Registro</bs-dropdown-header>
        <bs-dropdown-item href="/editar/1">Editar</bs-dropdown-item>
        <bs-dropdown-item disabled="true">Duplicar</bs-dropdown-item>
        <bs-dropdown-divider />
        <bs-dropdown-item>Excluir</bs-dropdown-item>
    </bs-dropdown-menu>
</bs-dropdown>
```

### `<bs-form-floating>` / `<bs-form-input-group>` / `<bs-form-check>` / `<bs-form-switch>` (FormGroup)

Containers de formulário — cada tag aplica a classe correspondente em um `<div>`:

| Tag | Classe(s) |
|---|---|
| `bs-form-floating` | `form-floating` |
| `bs-form-input-group` | `input-group` |
| `bs-form-input-group-sm` | `input-group input-group-sm` |
| `bs-form-input-group-lg` | `input-group input-group-lg` |
| `bs-form-check` | `form-check` |
| `bs-form-switch` | `form-check form-switch` |

```html
<bs-form-floating>
    <input asp-for="Nome" class="form-control" />
    <bs-form-label asp-for="Nome" />
    <span asp-validation-for="Nome" class="text-danger"></span>
</bs-form-floating>
```

### `<bs-form-label>` / `<bs-form-check-label>` (FormLabel)

Label integrado ao model binding (`asp-for`, obrigatório). `bs-form-label` aplica `control-label`; `bs-form-check-label` aplica `form-check-label`.

| Tag | Atributos |
|---|---|
| `bs-form-label` / `bs-form-check-label` | `asp-for` (obrigatório), `size:FontSizes`, `color:TextColors`, `custom-color:string`, `small:bool`, `italic:bool`, `monospace:bool` |

```html
<bs-form-label asp-for="Email" color="BodySecondary" small="true" />
```

### `<bs-icon>` (Icon)

Ícone do Bootstrap Icons (`<i class="bi bi-*">`). O atributo `icon` é obrigatório (o enum `BootstrapIcons` cobre a versão 1.11.3).

| Tag | Atributos |
|---|---|
| `bs-icon` | `icon:BootstrapIcons` (obrigatório), `color:TextColors`, `custom-color:string` |

```html
<bs-icon icon="HouseFill" color="Primary" />
<bs-icon icon="Alarm" custom-color="#cc0044" />
```

### `<bs-label>` / `<bs-check-label>` (Label)

Label genérico **sem** `asp-for` (para quando não há model binding). `bs-label` aplica `control-label`; `bs-check-label` aplica `form-check-label`. *[texto, com `custom-color`]* *[fundo]*

| Tag | Atributos |
|---|---|
| `bs-label` / `bs-check-label` | trait de texto (`custom-color`), `background-color:BackgroundColors`, `custom-bg:string`, `small:bool` |

```html
<bs-label font-weight="Semibold" color="Dark">Período</bs-label>
```

### `<bs-list-group>` (ListGroup)

Lista de itens (`ul`/`ol.list-group`), com itens estáticos ou acionáveis.

| Tag | Atributos |
|---|---|
| `bs-list-group` | `flush:bool`, `numbered:bool` (vira `<ol>` numerado), `horizontal:bool`, `breakpoint:ListGroupBreakpoints` (`Small`..`ExtraExtraLarge` → `list-group-horizontal-{sm..xxl}`; implica horizontal) |
| `bs-list-group-item` | `active:bool`, `disabled:bool`, `variant:ListGroupItemVariants` (`Primary`..`Dark`), `action:bool` (item acionável: `<a>` com `href`, senão `<button>`) |

```html
<bs-list-group>
    <bs-list-group-item action="true" href="/item/1" active="true">Item ativo</bs-list-group-item>
    <bs-list-group-item action="true" href="/item/2">Outro item</bs-list-group-item>
    <bs-list-group-item variant="Warning">Item destacado</bs-list-group-item>
</bs-list-group>
```

### `<bs-modal>` (Modal)

Diálogo modal — as camadas `modal-dialog`/`modal-content` são geradas automaticamente. `id` é gerado quando ausente (informe um para amarrar o trigger).

| Tag | Atributos |
|---|---|
| `bs-modal` | `fade:bool` (padrão `true`), `size:ModalSizes` (`Small`/`Large`/`ExtraLarge`), `fullscreen:ModalFullscreenModes` (`Always`, `SmallDown`..`ExtraExtraLargeDown`; precede `size`), `centered:bool`, `scrollable:bool`, `static-backdrop:bool` (não fecha com clique fora/Esc) |
| `bs-modal-header` | `title:string` (renderiza `h1.modal-title.fs-5`), `closeable:bool` (padrão `true`, gera btn-close) |
| `bs-modal-body` / `bs-modal-footer` | — |
| `bs-modal-trigger` | `target:string` (id do modal, obrigatório; aceita com/sem `#`), `variant:ButtonVariants` (padrão `Primary`) |

```html
<bs-modal-trigger target="confirmacao" variant="Danger">Excluir</bs-modal-trigger>

<bs-modal id="confirmacao" centered="true" static-backdrop="true">
    <bs-modal-header title="Confirmar exclusão" />
    <bs-modal-body>Essa ação não pode ser desfeita.</bs-modal-body>
    <bs-modal-footer>
        <bs-close-button dismiss="Modal" />
        <bs-button variant="Danger">Excluir</bs-button>
    </bs-modal-footer>
</bs-modal>
```

### `<bs-offcanvas>` (Offcanvas)

Painel lateral. `id` é gerado quando ausente (informe um para amarrar o trigger).

| Tag | Atributos |
|---|---|
| `bs-offcanvas` | `placement:OffcanvasPlacements` (`Start`/`End`/`Top`/`Bottom`; padrão `Start`), `static-backdrop:bool`, `body-scroll:bool` (`data-bs-scroll="true"`) |
| `bs-offcanvas-header` | `title:string` (renderiza `h5.offcanvas-title`), `closeable:bool` (padrão `true`) |
| `bs-offcanvas-body` | — |
| `bs-offcanvas-trigger` | `target:string` (obrigatório; aceita com/sem `#`), `variant:ButtonVariants` (padrão `Primary`) |

```html
<bs-offcanvas-trigger target="menu-lateral" variant="Secondary">Menu</bs-offcanvas-trigger>

<bs-offcanvas id="menu-lateral" placement="End">
    <bs-offcanvas-header title="Navegação" />
    <bs-offcanvas-body>Links do menu...</bs-offcanvas-body>
</bs-offcanvas>
```

### `<bsc-page-header>` (PageHeader)

Componente custom Maxsys (prefixo `bsc-`): cabeçalho de página com título e subtítulo. *[texto]* *[fundo]* (nas três tags)

| Tag | Atributos |
|---|---|
| `bsc-page-header` | `alignment:PageTitleAlignments` (`Start`/`Center`/`End`; padrão `Start`) + traits |
| `bsc-title` (renderiza `h1`) / `bsc-sub-title` (renderiza `h2`) | traits |

Defaults notáveis: `FontWeight = Light`, `TextSize = Size4` (título) / `Size5` (subtítulo).

```html
<bsc-page-header alignment="Center">
    <bsc-title>Produtos</bsc-title>
    <bsc-sub-title>Gerenciamento de catálogo</bsc-sub-title>
</bsc-page-header>
```

### `<bs-pagination>` (Pagination)

Paginação com markup manual (`nav > ul.pagination`).

| Tag | Atributos |
|---|---|
| `bs-pagination` | `label:string` (aria-label do nav; padrão `Page navigation`), `size:PaginationSizes` (`Small`/`Large`), `justify:PaginationJustifications` (`Start`/`Center`/`End`) |
| `bs-page-item` | `href:string` (sem href — ou desabilitado — renderiza `span.page-link`), `active:bool` (+ `aria-current="page"`), `disabled:bool` |

```html
<bs-pagination justify="Center">
    <bs-page-item disabled="true">&laquo;</bs-page-item>
    <bs-page-item href="?page=1" active="true">1</bs-page-item>
    <bs-page-item href="?page=2">2</bs-page-item>
    <bs-page-item href="?page=2">&raquo;</bs-page-item>
</bs-pagination>
```

> Para paginação calculada a partir de página atual/total (com reticências), veja o ViewComponent [`vc:bs-pagination`](#vcbs-pagination).

### `<bs-placeholder>` (Placeholder)

Esqueleto de carregamento. *[fundo, com `bg`]*

| Tag | Atributos |
|---|---|
| `bs-placeholder` | `col:int?` (1–12 → `col-*`), `size:PlaceholderSizes` (`ExtraSmall`/`Small`/`Large`), `bg:BackgroundColors`, `custom-bg:string` |
| `bs-placeholder-glow` / `bs-placeholder-wave` | contêiner de animação (renderiza `p.placeholder-glow`/`p.placeholder-wave`) |
| `bs-placeholder-button` | `variant:ButtonVariants` (padrão `Primary`), `col:int?` (1–12) |

```html
<bs-placeholder-glow>
    <bs-placeholder col="7" />
    <bs-placeholder col="4" size="Small" bg="Secondary" />
</bs-placeholder-glow>

<bs-placeholder-button variant="Primary" col="4" />
```

### `<bs-progress>` (Progress)

Barra de progresso: o wrapper (`bs-progress`) define valor/min/max e a barra (`bs-progress-bar`) o visual. O percentual é calculado automaticamente. Suporta barras empilhadas via `bs-progress-stacked` (nesse caso o `width` vai no wrapper).

| Tag | Atributos |
|---|---|
| `bs-progress` | `value:double` (padrão 0), `min:double` (padrão 0), `max:double` (padrão 100), `height:int?` (px) |
| `bs-progress-bar` | `bg:BackgroundColors`, `custom-bg:string`, `striped:bool`, `animated:bool` (implica striped), `show-label:bool` (exibe o percentual quando não há conteúdo) |
| `bs-progress-stacked` | contêiner para múltiplas `bs-progress` empilhadas |

```html
<bs-progress value="65" height="20">
    <bs-progress-bar bg="Success" striped="true" animated="true" show-label="true" />
</bs-progress>

<bs-progress-stacked>
    <bs-progress value="30"><bs-progress-bar bg="Success" /></bs-progress>
    <bs-progress value="20"><bs-progress-bar bg="Warning" /></bs-progress>
</bs-progress-stacked>
```

### `<bs-spinner>` (Spinner)

Indicador de carregamento nas variações *border* (padrão) e *grow*.

| Tag | Atributos |
|---|---|
| `bs-spinner` | `grow:bool` (spinner-grow), `small:bool` (spinner-*-sm), `label:string` (texto acessível visually-hidden; padrão `Loading...`), `no-status:bool` (uso inline em botões: sem `role="status"`, com `aria-hidden`), `text-color:TextColors`, `custom-fg:string` |

```html
<bs-spinner text-color="Primary" label="Carregando..." />

<bs-button variant="Primary" disabled="true">
    <bs-spinner small="true" no-status="true" /> Processando...
</bs-button>
```

### `<bs-vstack>` / `<bs-hstack>` (Stack)

Pilhas de layout vertical/horizontal (`div.vstack`/`div.hstack`).

| Tag | Atributos |
|---|---|
| `bs-vstack` / `bs-hstack` | `gap:int` (0–5) |

```html
<bs-hstack gap="3">
    <bs-button variant="Primary">Salvar</bs-button>
    <bs-button variant="Secondary" outline="true">Cancelar</bs-button>
</bs-hstack>
```

### `<bs-tab>` (Tab)

Abas (`ul.nav.nav-tabs` + `div.tab-content`) montadas automaticamente a partir dos itens — o primeiro item vira o ativo. `class` em header/content é repassado ao wrapper interno.

```html
<bs-tab>
    <bs-tab-item>
        <bs-tab-item-header>Geral</bs-tab-item-header>
        <bs-tab-item-content>Conteúdo da aba Geral.</bs-tab-item-content>
    </bs-tab-item>
    <bs-tab-item>
        <bs-tab-item-header>Avançado</bs-tab-item-header>
        <bs-tab-item-content>Conteúdo da aba Avançado.</bs-tab-item-content>
    </bs-tab-item>
</bs-tab>
```

### `<bs-table>` (Table)

Tabela completa: `bs-table` > `bs-thead`/`bs-tbody` > `bs-tr` > `bs-th`/`bs-td`. Todas as tags aceitam os traits de texto/fundo (via CSS vars `--bs-table-*` para cores custom) e alinhamento via atributos de prefixo `align-*`.

Atributos **comuns** a `bs-table`, `bs-thead`, `bs-tbody`, `bs-tr`, `bs-td`, `bs-th`:

* `type:TableTypes` (`Primary`..`Dark` → `table-*`) — exceto em `bs-table`
* `border-color:BorderColors`, `bordered:bool`
* traits: `custom-fg`, `font-weight`, `text-transform`, `text-size`, `text-color`, `italic`, `monospace`, `background-color`, `custom-bg`
* alinhamento: `align-start` / `align-center` / `align-end` (texto) e `align-top` / `align-middle` / `align-bottom` (vertical), como atributos booleanos (ex.: `align-center="true"`); em `bs-table` o alinhamento é herdado por thead/tbody

Exclusivos de `bs-table`: `small`, `striped`, `striped-columns`, `hover`, `borderless`, `responsive` (embrulha em `div.table-responsive`), `divider` (aplica `table-group-divider` no tbody), `shadow`, `caption-top`. Exclusivo de `bs-tbody`: `divider:bool`.

```html
<bs-table striped="true" hover="true" responsive="true" align-middle="true">
    <bs-thead type="Dark">
        <bs-tr>
            <bs-th>Nome</bs-th>
            <bs-th align-end="true">Valor</bs-th>
        </bs-tr>
    </bs-thead>
    <bs-tbody divider="true">
        <bs-tr>
            <bs-td>Item A</bs-td>
            <bs-td align-end="true">R$ 10,00</bs-td>
        </bs-tr>
    </bs-tbody>
</bs-table>
```

> Dica (do próprio código-fonte): para temas custom, considere adicionar `.table { --bs-table-bg: transparent; }` no CSS da aplicação.

### `<bs-toast>` (Toast)

Notificações toast com container posicionador. `id` do toast é gerado quando ausente. A **exibição** do toast é via JS da aplicação (`new bootstrap.Toast(el).show()`).

| Tag | Atributos |
|---|---|
| `bs-toast-container` | `position:ToastContainerPositions` (`TopLeft`..`BottomRight`, 9 posições; aplica `position-fixed` + utilitários) |
| `bs-toast` | `autohide:bool` (padrão `true`; `data-bs-autohide` só é emitido quando difere do padrão), `delay:int?` (ms → `data-bs-delay`) |
| `bs-toast-header` | `title:string` (renderiza `strong.me-auto`), `closeable:bool` (padrão `true`) |
| `bs-toast-body` | — |

```html
<bs-toast-container position="BottomRight">
    <bs-toast id="toast-salvo" delay="5000">
        <bs-toast-header title="Sucesso" />
        <bs-toast-body>Registro salvo.</bs-toast-body>
    </bs-toast>
</bs-toast-container>
```

### `bs-tooltip` / `bs-popover` (Tooltip/Popover)

Diferente dos demais, são **atributos globais** aplicáveis a qualquer elemento — adicionam os `data-bs-*` correspondentes.

| Atributo | Efeito |
|---|---|
| `bs-tooltip="texto"` | `data-bs-toggle="tooltip"` + `data-bs-title` |
| `bs-tooltip-placement:TooltipPlacements` | `data-bs-placement` (`Top`/`Bottom`/`Left`/`Right`) |
| `bs-popover="conteúdo"` | `data-bs-toggle="popover"` + `data-bs-content` |
| `bs-popover-title:string` | `data-bs-title` |
| `bs-popover-placement:PopoverPlacements` | `data-bs-placement` (`Top`/`Bottom`/`Left`/`Right`) |

```html
<bs-button variant="Secondary" bs-tooltip="Salva o registro" bs-tooltip-placement="Top">Salvar</bs-button>

<bs-button variant="Info" bs-popover="Conteúdo detalhado aqui." bs-popover-title="Ajuda">?</bs-button>
```

> :warning: **Inicialização JS obrigatória** — no Bootstrap, tooltips e popovers são *opt-in* por performance. Inclua na página:
>
> ```html
> <script>
>     const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
>     [...tooltipTriggerList].map(el => new bootstrap.Tooltip(el));
>
>     const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
>     [...popoverTriggerList].map(el => new bootstrap.Popover(el));
> </script>
> ```

## ViewComponents (data-driven)

Enquanto os TagHelpers projetam **conteúdo escrito na view**, os ViewComponents recebem um **modelo** e geram o markup completo. Regra prática: tem uma coleção/estado vindo do controller → use o ViewComponent; precisa de conteúdo custom por item → use o TagHelper. Os pares coexistem — `vc:bs-pagination`/`bs-pagination`, `vc:bs-breadcrumb`/`bs-breadcrumb`, `vc:bs-carousel`/`bs-carousel`.

Requerem `services.AddControllersWithViews().AddMaxsysBootstrap()` (ver [Setup](#setup)).

### `vc:bs-pagination`

Recebe página atual/total e um template de URL; renderiza a paginação completa: botões anterior/próxima (« »), janela de páginas com reticências, primeira e última página sempre visíveis.

| Parâmetro | Tipo | Descrição |
|---|---|---|
| `current-page` | `int` | Página atual (clampada em 1..totalPages). |
| `total-pages` | `int` | Total de páginas. |
| `url-format` | `string` | Template da URL com `{0}` para o número da página (obrigatório). |
| `label` | `string?` | aria-label do nav (padrão `Navegação de páginas`). |
| `size` | `PaginationViewSizes?` | `Small`/`Large`. |
| `justify` | `JustifyContents?` | `Start`/`Center`/`End`/`Between`/`Arround`/`Evenly`. |
| `max-visible-pages` | `int?` | Tamanho da janela de páginas (padrão 7, mínimo 3). |

```html
<vc:bs-pagination current-page="Model.Page"
                  total-pages="Model.TotalPages"
                  url-format="?page={0}"
                  justify="Center"
                  max-visible-pages="5" />
```

Defaults globais: `BsPaginationViewDefaults` (`Label`, `Size`, `Justify`, `MaxVisiblePages`).

### `vc:bs-breadcrumb`

Recebe a lista de itens e renderiza a trilha completa — o último item (e itens sem URL) são marcados como ativos com `aria-current="page"`.

| Parâmetro | Tipo | Descrição |
|---|---|---|
| `items` | `IEnumerable<BreadcrumbItem>` | Itens da trilha (obrigatório). |
| `divider` | `string?` | Divisor custom (CSS var `--bs-breadcrumb-divider`); `null` usa o padrão do Bootstrap (`/`). |

Modelo — `record BreadcrumbItem(string Text, string? Url = null)`:

```csharp
// Controller/ViewModel
public BreadcrumbItem[] Trail { get; } =
[
    new("Home", "/"),
    new("Biblioteca", "/lib"),
    new("Dados") // sem URL: item ativo
];
```

```html
<vc:bs-breadcrumb items="Model.Trail" divider=">" />
```

Defaults globais: `BsBreadcrumbViewDefaults.Divider`.

### `vc:bs-carousel`

Recebe a lista de slides e renderiza a estrutura completa — ao contrário do TagHelper, os **indicators são contados automaticamente** a partir do modelo e o primeiro slide já sai ativo. Imagens saem com `d-block w-100` e legendas com `carousel-caption d-none d-md-block`.

| Parâmetro | Tipo | Descrição |
|---|---|---|
| `slides` | `IEnumerable<CarouselSlide>` | Slides (obrigatório). |
| `id` | `string?` | Id do carousel (gerado quando ausente). |
| `fade` | `bool?` | Transição de fade. |
| `controls` | `bool?` | Botões prev/next (padrão `true`). |
| `indicators` | `bool?` | Indicators (padrão `false`). |
| `autoplay` | `CarouselAutoplayModes?` | `None`/`OnLoad` (`data-bs-ride="carousel"`)/`AfterInteraction` (`data-bs-ride="true"`). |
| `interval-ms` | `int?` | Intervalo por slide em ms (0 usa o padrão do Bootstrap, 5000). |

Modelo — `record CarouselSlide(string ImageUrl, string? Alt = null, string? CaptionTitle = null, string? CaptionText = null)`:

```csharp
public CarouselSlide[] Slides { get; } =
[
    new("/img/1.jpg", "Slide 1", CaptionTitle: "Bem-vindo", CaptionText: "Primeiro destaque"),
    new("/img/2.jpg", "Slide 2")
];
```

```html
<vc:bs-carousel slides="Model.Slides" controls="true" indicators="true" autoplay="OnLoad" />
```

Defaults globais: `BsCarouselViewDefaults` (`Fade`, `Controls`, `Indicators`, `Autoplay`, `IntervalMs`).

---

#### [README](README.md)
