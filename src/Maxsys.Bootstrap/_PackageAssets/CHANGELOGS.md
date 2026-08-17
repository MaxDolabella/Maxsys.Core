# Maxsys.Bootstrap

## :mortar_board: Cada lançamento é um novo aprendizado!!

### 0.1.0
* :sparkles: Novos TagHelpers, cobrindo os componentes Bootstrap 5.3: **Alert**\*, **Badge**\*, **Breadcrumb**\*, **Button**, **ButtonGroup/Toolbar**, **Card**, **Carousel**, **CloseButton**, **Collapse**, **Dropdown**, **ListGroup**, **Modal**, **Offcanvas**, **Pagination**, **Placeholder**, **Progress**, **Spinner**, **Toast** e **Tooltip/Popover** (atributos globais `bs-tooltip`/`bs-popover`). (\* revisados/ampliados)
* :sparkles: Novos **ViewComponents** *data-driven*: `vc:bs-pagination` (janela de páginas com reticências), `vc:bs-breadcrumb` (modelo `BreadcrumbItem`) e `vc:bs-carousel` (modelo `CarouselSlide`, indicators com contagem automática).
* :sparkles: `AddMaxsysBootstrap()` (`IMvcBuilder`): registra o assembly como *Application Part* do MVC, habilitando a descoberta dos ViewComponents.
* :hammer_and_wrench: Projeto convertido para o padrão `_PackageAssets` com metadados NuGet centralizados (`Directory.Build.targets`/`Directory.Packages.props`).
* :hammer_and_wrench: Componentes interativos (Modal, Toast, Collapse, Carousel, Dropdown, Offcanvas, Tooltip/Popover) emitem apenas atributos `data-bs-*` — o JS do Bootstrap é referenciado pela aplicação (tooltips/popovers exigem inicialização manual via JS).

---
### 0.0.5
* :warning: Target alterado para **.NET 10** (era .NET 9).

---
### 0.0.1
* Primeiro lançamento.
