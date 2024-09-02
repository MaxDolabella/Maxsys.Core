# Maxsys.Bootstrap

:building_construction: Documentação em construção...

## Índice

<!-- * [xxxxxxxxx](#xxxxxxxxx) --> <!-- <a id="XXXXXXXXXXXXXXXXXXXX"></a> -->
* [Tag Helpers](#TagHelpers)
    * [Accordion](#Accordion)
    * [Alert](#Alert)
    * [Badge](#Badge)
    * [Breadcrumb](#Breadcrumb)
    * [Divider](#Divider)
    * [Icon](#Icon)
    * [Stack](#Stack)
    * [Table](#Table)
    * [Tab](#Tab)
    
* [Form Inputs](#FormInputs)
    * [CheckInput (asp-for)](#CheckInput)
    * [FormGroup (input-group / form-check)](#FormGroup)
    * [FormLabel](#FormLabel)

* [Componentes](#Componentes)
    * [PageHeader](#PageHeader)

* [Enums](#Enums)
    * [BackgroundColors](#Enums-BackgroundColors)
    * [BorderColors](#Enums-BorderColors)
    * [FontSizes](#Enums-FontSizes)
    * [FontWeights](#Enums-FontWeights)
    * [TextColors](#Enums-TextColors)

* [Exemplos](#Exemplos)
    * [bs-form-check / bs-form-switch](#Exemplos-form-check-switch)
    * [bs-form-floating](#Exemplos-bs-form-floating)


## TagHelpers <a id="TagHelpers"></a>

### Accordion <a id="Accordion"></a>
[Docs](https://getbootstrap.com/docs/5.3/components/accordion/)

```html
<bs-accordion>
    <bs-accordion-item>
        <bs-accordion-header></bs-accordion-header>
        <bs-accordion-body></bs-accordion-body>
    </bs-accordion-item>
</bs-accordion>
```

#### Atributos
* bs-accordion
    * `flush:bool`
    * `always-open:bool`
* bs-accordion-header
    * `class / style`
* bs-accordion-body
    * `class / style`

#### Defaults
```csharp
AccordionDefaults.IsFlush = false;
AccordionDefaults.IsAwaysOpen = false;
```

---
### Alert <a id="Alert"></a>
[Docs](https://getbootstrap.com/docs/5.3/components/alerts/)

```html
<bs-alert>
    <bs-alert-header></bs-alert-header>
</bs-alert>
```

#### Atributos
* bs-alert
    * `type:AlertTypes`
    * `icon:BootstrapIcons`
    * `custom-bg:string?`
    * `text-transform:TextTransformations`
    * `font-weight:FontWeights`
    * `text-size:FontSizes`
    * `text-color:TextColors`
    * `custom-fg:string?`
    * `small:bool`
    * `italic:bool`
    * `monospace:bool`
* bs-accordion-header
    * `class / style`
    * `text-transform:TextTransformations`
    * `font-weight:FontWeights`
    * `text-size:FontSizes`
    * `text-color:TextColors`
    * `custom-fg:string?`
    * `small:bool`
    * `italic:bool`
    * `monospace:bool`
* bs-accordion-body
    * `class / style`

#### Defaults
```csharp
AlertDefaults.Type = AlertTypes.Primary;
AlertDefaults.Icon = BootstrapIcons.None;
AlertDefaults.CustomBackgroundColor = null;
AlertDefaults.TextTransform = TextTransformations.None;
AlertDefaults.FontWeight = FontWeights.None;
AlertDefaults.TextSize = FontSizes.None;
AlertDefaults.TextColor = TextColors.None;
AlertDefaults.CustomTextColor = null;
AlertDefaults.IsSmall = false;
AlertDefaults.IsItalic = false;
AlertDefaults.IsMonospace = false;

AlertHeaderDefaults.TextTransform = TextTransformations.None;
AlertHeaderDefaults.FontWeight = FontWeights.None;
AlertHeaderDefaults.TextSize = FontSizes.None;
AlertHeaderDefaults.TextColor = TextColors.None;
AlertHeaderDefaults.CustomTextColor = null;
AlertHeaderDefaults.IsSmall = false;
AlertHeaderDefaults.IsItalic = false;
AlertHeaderDefaults.IsMonospace = false;
```

#### Enums

##### AlertTypes
```
Primary
Secondary
Success
Danger
Warning
Info
Light
Dark
BodySecondary
BodyTertiary
Body
Black
White
```

---
### Badge <a id="Badge"></a>
[Docs](https://getbootstrap.com/docs/5.3/components/badge/)

```html
<bs-badge></bs-badge>
```

#### Atributos
* bs-badge
    * `type:BadgeTypes`
    * `rounded:bool`
    * `custom-bg:string?`
    * `size:FontSizes`
    * `custom-fg:string?`
    * `small:bool`
    * `italic:bool`
    * `monospace:bool`

#### Defaults
```csharp
BadgeDefaults.Type = BadgeTypes.Primary;
BadgeDefaults.IsRounded = false;
BadgeDefaults.CustomBackgroundColor = null;
BadgeDefaults.TextSize = FontSizes.None;
BadgeDefaults.CustomTextColor = null;
BadgeDefaults.IsSmall = false;
BadgeDefaults.IsItalic = false;
BadgeDefaults.IsMonospace = false;
```

#### Enums

##### BadgeTypes
```
None
Primary
PrimarySubtle
Secondary
SecondarySubtle
Success
SuccessSubtle
Danger
DangerSubtle
Warning
WarningSubtle
Info
InfoSubtle
Light
LightSubtle
Dark
DarkSubtle
BodySecondary
BodyTertiary
Body
Black
White
```

---
### Breadcrumb <a id="Breadcrumb"></a>
[Docs](Breadcrumb)

```html
<bs-breadcrumb>
    <bs-breadcrumb-item></bs-breadcrumb-item>
</bs-breadcrumb>
```

#### Atributos
* bs-breadcrumb
    * `divider:string`
    * `text-transform:TextTransformations`
    * `font-weight:FontWeights`
    * `text-size:FontSizes`
    * `text-color:TextColors`
    * `custom-color:string?`
    * `small:bool`
    * `italic:bool`
    * `monospace:bool`
* bs-breadcrumb-item
    * `active:bool`
    * `text-transform:TextTransformations`
    * `font-weight:FontWeights`
    * `text-size:FontSizes`
    * `text-color:TextColors`
    * `custom-color:string?`
    * `small:bool`
    * `italic:bool`
    * `monospace:bool`

#### Defaults
```csharp
BreadcrumbDefaults.Divider = "/";
BreadcrumbDefaults.TextTransform = TextTransformations.None;
BreadcrumbDefaults.TextSize = FontSizes.None;
BreadcrumbDefaults.FontWeight = FontWeights.None;
BreadcrumbDefaults.TextColor = TextColors.None;
BreadcrumbDefaults.CustomTextColor = null;
BreadcrumbDefaults.IsSmall = false;
BreadcrumbDefaults.IsItalic = false;
BreadcrumbDefaults.IsMonospace = false;

BreadcrumbItemDefaults.IsActive = false;
BreadcrumbItemDefaults.TextTransform = TextTransformations.None;
BreadcrumbItemDefaults.TextSize = FontSizes.None;
BreadcrumbItemDefaults.FontWeight = FontWeights.None;
BreadcrumbItemDefaults.TextColor = TextColors.None;
BreadcrumbItemDefaults.CustomTextColor = null;
BreadcrumbItemDefaults.IsSmall = false;
BreadcrumbItemDefaults.IsItalic = false;
BreadcrumbItemDefaults.IsMonospace = false;
```

---
### Divider <a id="Divider"></a>
[Docs](#)

```html
<!-- vertical divider -->
<bs-vr />

<!-- horizontal divider -->
<bs-hr />
```

#### Atributos
* bs-vr / bs-hr
    * `class`
    * `color:TextColors`
    * `custom-color:string?`
    * `thickness:string?` - ex.: "25px"

#### Defaults
```csharp
DividerDefaults.Color = TextColors.None;
DividerDefaults.CustomForeground = null;
DividerDefaults.Thickness = null;
```

---
### Icon <a id="Icon"></a>
[Docs](https://icons.getbootstrap.com/)

```html
<bs-icon icon=""/>
```

#### Atributos
* bs-icon
    * `class / style`
    * `icon:BootstrapIcons` (obrigatório)
    * `color:TextColors`
    * `custom-color:string?`

#### Defaults
```csharp
IconDefaults.Color = TextColors.None;
IconDefaults.CustomColor = null;
```

---
### Stack <a id="Stack"></a>
[Docs](https://getbootstrap.com/docs/5.3/helpers/stacks/)

```html
<bs-vstack></bs-vstack>

<bs-hstack></bs-hstack>
```

#### Atributos
* bs-vstack / bs-hstack
    * `gap:int` - (0 - 5)

---
### Tab <a id="Tab"></a>
[Docs](https://getbootstrap.com/docs/5.3/components/navs-tabs/)

```html
<bs-tab>
    <bs-tab-item>
        <bs-tab-item-header></bs-tab-item-header>
        <bs-tab-item-content></bs-tab-item-content>
    </bs-tab-item>
</bs-tab>
```

---
### Table <a id="Table"></a>
[Docs](https://getbootstrap.com/docs/5.3/content/tables/)

> Necessário adicionar seguinte css:  
.table {
    --bs-table-bg: transparent;
}

```html
<bs-table>
    <bs-thead>
        <bs-tr>
            <bs-th></bs-th>
        </bs-tr>
    </bs-thead>
    
    <bs-tbody>
         <bs-tr>
            <bs-td></bs-td>
        </bs-tr>
    </bs-tbody>
    
</bs-table>
```

#### Atributos

* bs-table / bs-thead / bs-tbody / bs-tr / bs-td / bs-th (Comum)
    * `class / style`
    * `custom-fg:string?`
    * `font-weight:FontWeights`
    * `text-transform:TextTransformations`
    * `text-size:FontSizes`
    * `text-color:TextColors`
    * `italic:bool`
    * `monospace:bool`
    * `background-color:BackgroundColors`
    * `custom-bg:string?`
    * `type:TableTypes`
    * `border-color:BorderColors`
    * `bordered:bool`
    * `align-*`
        * `start` / `center` / `end`
        * `top` / `middle` / `bottom`

* bs-table (exclusive)
    * `small:bool`
    * `striped:bool`
    * `striped-columns:bool`
    * `hover:bool`
    * `borderless:bool`
    * `responsive:bool`
    * `divider:bool`
    * `shadow:bool`
    * `caption-top:bool`
    
* bs-tbody (exclusive)
    * `divider:bool`
    

#### Defaults
```csharp
TableDefaults.CustomTextColor = null;
TableDefaults.FontWeight = FontWeights.None;
TableDefaults.TextTransform = TextTransformations.None;
TableDefaults.TextSize = FontSizes.None;
TableDefaults.TextColor = TextColors.None;
TableDefaults.IsItalic = false;
TableDefaults.IsMonospace = false;
TableDefaults.BackgroundColor = BackgroundColors.None;
TableDefaults.CustomBackgroundColor = null;
TableDefaults.BorderColor = BorderColors.None;
TableDefaults.IsSmall = false;
TableDefaults.IsStriped = false;
TableDefaults.IsStripedColumns = false;
TableDefaults.IsHover = false;
TableDefaults.HasBorder = false;
TableDefaults.IsBorderless = false;
TableDefaults.IsResponsive = false;
TableDefaults.HasDivider = false;
TableDefaults.HasShadow = false;
TableDefaults.IsCaptionTop = false;

TableHeaderDefaults.CustomTextColor = null;
TableHeaderDefaults.FontWeight = FontWeights.None;
TableHeaderDefaults.TextTransform = TextTransformations.None;
TableHeaderDefaults.TextSize = FontSizes.None;
TableHeaderDefaults.TextColor = TextColors.None;
TableHeaderDefaults.IsItalic = false;
TableHeaderDefaults.IsMonospace = false;
TableHeaderDefaults.BackgroundColor = BackgroundColors.None;
TableHeaderDefaults.CustomBackgroundColor = null;
TableHeaderDefaults.Type = TableTypes.None;
TableHeaderDefaults.BorderColor = BorderColors.None;
TableHeaderDefaults.HasBorder = false;
    
TableBodyDefaults.CustomTextColor = null;
TableBodyDefaults.FontWeight = FontWeights.None;
TableBodyDefaults.TextTransform = TextTransformations.None;
TableBodyDefaults.TextSize = FontSizes.None;
TableBodyDefaults.TextColor = TextColors.None;
TableBodyDefaults.IsItalic = false;
TableBodyDefaults.IsMonospace = false;
TableBodyDefaults.BackgroundColor = BackgroundColors.None;
TableBodyDefaults.CustomBackgroundColor = null;
TableBodyDefaults.Type = TableTypes.None;
TableBodyDefaults.class="table-group-divider">
TableBodyDefaults.BorderColor = BorderColors.None;
TableBodyDefaults.HasBorder = false;

TableRowDefaults.CustomTextColor = null;
TableRowDefaults.FontWeight = FontWeights.None;
TableRowDefaults.TextTransform = TextTransformations.None;
TableRowDefaults.TextSize = FontSizes.None;
TableRowDefaults.TextColor = TextColors.None;
TableRowDefaults.IsItalic = false;
TableRowDefaults.IsMonospace = false;
TableRowDefaults.BackgroundColor = BackgroundColors.None;
TableRowDefaults.CustomBackgroundColor = null;
TableRowDefaults.Type = TableTypes.None;
TableRowDefaults.BorderColor = BorderColors.None;
TableRowDefaults.HasBorder = false;

TableColumnDefaults.CustomTextColor = null;
TableColumnDefaults.FontWeight = FontWeights.None;
TableColumnDefaults.TextTransform = TextTransformations.None;
TableColumnDefaults.TextSize = FontSizes.None;
TableColumnDefaults.TextColor = TextColors.None;
TableColumnDefaults.IsItalic = false;
TableColumnDefaults.IsMonospace = false;
TableColumnDefaults.BackgroundColor = BackgroundColors.None;
TableColumnDefaults.CustomBackgroundColor = null;
TableColumnDefaults.Type = TableTypes.None;
TableColumnDefaults.BorderColor = BorderColors.None;
TableColumnDefaults.HasBorder = false;

TableHeadColumnDefaults.CustomTextColor = null;
TableHeadColumnDefaults.FontWeight = FontWeights.None;
TableHeadColumnDefaults.TextTransform = TextTransformations.None;
TableHeadColumnDefaults.TextSize = FontSizes.None;
TableHeadColumnDefaults.TextColor = TextColors.None;
TableHeadColumnDefaults.IsItalic = false;
TableHeadColumnDefaults.IsMonospace = false;
TableHeadColumnDefaults.BackgroundColor = BackgroundColors.None;
TableHeadColumnDefaults.CustomBackgroundColor = null;
TableHeadColumnDefaults.Type = TableTypes.None;
TableHeadColumnDefaults.BorderColor = BorderColors.None;
TableHeadColumnDefaults.HasBorder = false;
```

---
## Form Inputs <a id="FormInputs"></a>

### CheckInput (asp-for) <a id="CheckInput"></a>
[Docs](#)

```html
<!-- checkbox -->
<bs-input-check asp-for=""></bs-input-check>

<!-- switch -->
<bs-input-switch asp-for=""></bs-input-check>
```

---
### FormGroup <a id="FormGroup"></a>
[Docs](#)

```html
<!-- form-floating -->
<bs-form-floating></bs-form-floating>

<!-- input-group (-sm -lg) -->
<bs-form-input-group></bs-form-input-group>
<bs-form-input-group-sm></bs-form-input-group-sm>
<bs-form-input-group-lg></bs-form-input-group-lg>

<!-- form-check -->
<bs-form-check></bs-form-check>

<!-- form-switch -->
<bs-form-switch></bs-form-switch>
```

---
### FormLabel <a id="FormLabel"></a>
[Docs](#)

```html
<bs-form-label></bs-form-label>

<bs-form-check-label></bs-form-check-label>
```

#### Atributos
* bs-form-label / bs-form-check-label
    * `class / style`
    * `size:FontSizes`
    * `color:TextColors`
    * `custom-color:string?`
    * `small:bool`
    * `italic:bool`
    * `monospace:bool`
    

#### Defaults
```csharp
FormLabelDefaults.Size = FontSizes.None;
FormLabelDefaults.Color = TextColors.None;
FormLabelDefaults.CustomColor = null;
FormLabelDefaults.IsSmall = false;
FormLabelDefaults.IsItalic = false;
FormLabelDefaults.IsMonospace = false;
```

#### Enums

##### TableStyle
```
None
Primary
Secondary
Success
Danger
Warning
Info
Light
Dark
```

---
## Componentes <a id="Componentes"></a>

### PageHeader <a id="PageHeader"></a>

```html
<bsc-page-header>
    <bsc-title></bsc-title>
    <bsc-sub-title></bsc-sub-title>
</bsc-page-header>
```

#### Atributos
* bsc-page-header / bsc-title / bsc-sub-title (Comum)
    * `class / style`
    * `custom-fg:string?`
    * `font-weight:FontWeights`
    * `text-transform:TextTransformations`
    * `text-size:FontSizes`
    * `text-color:TextColors`
    * `italic:bool`
    * `monospace:bool`
    * `background-color:BackgroundColors`
    * `custom-bg:string?`

* bsc-page-header (Exclusive)
    * `alignment:PageTitleAlignments`
    

#### Defaults
```csharp
PageHeaderDefaults.Alignment = PageTitleAlignments.Start;
PageHeaderDefaults.CustomTextColor = null;
PageHeaderDefaults.FontWeight = FontWeights.Light;
PageHeaderDefaults.TextTransform = TextTransformations.None;
PageHeaderDefaults.TextSize = FontSizes.Size4;
PageHeaderDefaults.TextColor = TextColors.None;
PageHeaderDefaults.IsItalic = false;
PageHeaderDefaults.IsMonospace = false;
PageHeaderDefaults.BackgroundColor = BackgroundColors.None;
PageHeaderDefaults.CustomBackgroundColor = null;

PageHeaderTitleDefaults.CustomTextColor = null;
PageHeaderTitleDefaults.FontWeight = FontWeights.Light;
PageHeaderTitleDefaults.TextTransform = TextTransformations.None;
PageHeaderTitleDefaults.TextSize = FontSizes.Size4;
PageHeaderTitleDefaults.TextColor = TextColors.None;
PageHeaderTitleDefaults.IsItalic = false;
PageHeaderTitleDefaults.IsMonospace = false;
PageHeaderTitleDefaults.BackgroundColor = BackgroundColors.None;
PageHeaderTitleDefaults.CustomBackgroundColor = null;

PageHeaderSubTitleDefaults.CustomTextColor = null;
PageHeaderSubTitleDefaults.FontWeight = FontWeights.Light;
PageHeaderSubTitleDefaults.TextTransform = TextTransformations.None;
PageHeaderSubTitleDefaults.TextSize = FontSizes.Size5;
PageHeaderSubTitleDefaults.TextColor = TextColors.None;
PageHeaderSubTitleDefaults.IsItalic = false;
PageHeaderSubTitleDefaults.IsMonospace = false;
PageHeaderSubTitleDefaults.BackgroundColor = BackgroundColors.None;
PageHeaderSubTitleDefaults.CustomBackgroundColor = null;
```

#### Enums

##### PageTitleAlignments
```
Center
Start
End
```

---
## Enums <a id="Enums"></a>

### BackgroundColors <a id="Enums-BackgroundColors"></a>
```
Primary
PrimarySubtle
Secondary
SecondarySubtle
Success
SuccessSubtle
Danger
DangerSubtle
Warning
WarningSubtle
Info
InfoSubtle
Light
LightSubtle
Dark
DarkSubtle
BodySecondary
BodyTertiary
Body
Black
White
Transparent
```

### BorderColors <a id="Enums-BorderColors"></a>
```
Primary
PrimarySubtle
Secondary
SecondarySubtle
Success
SuccessSubtle
Danger
DangerSubtle
Warning
WarningSubtle
Info
InfoSubtle
Light
LightSubtle
Dark
DarkSubtle
Black
White
```

### FontSizes <a id="Enums-FontSizes"></a>
```
Size1
Size2
Size3
Size4
Size5
Size6
```

### FontWeights <a id="Enums-FontWeights"></a>
```
Bold
Bolder
Semibold
Medium
Normal
LightLighter
```

### TextColors <a id="Enums-TextColors"></a>
```
Primary
PrimaryEmphasis
Secondary
SecondaryEmphasis
Success
SuccessEmphasis
Danger
DangerEmphasis
Warning
WarningEmphasis
Info
InfoEmphasis
Light
LightEmphasis
Dark
DarkEmphasis
Body
BodyEmphasis
BodySecondary
BodyTertiary
Black
White
Black50
White50
```

## Exemplos <a id="Exemplos"></a>

### bs-form-check / bs-form-switch <a id="Exemplos-form-check-switch"></a>
```html
<!-- bs-form-check -->
<bs-form-check>
    <bs-form-check-label asp-for="BooleanProp" />
    <bs-input-check asp-for="BooleanProp" />
    <span asp-validation-for="BooleanProp" class="text-danger"></span>
</bs-form-check>

<!-- bs-form-switch -->
<bs-form-switch>
    <bs-form-check-label asp-for="BooleanProp" />
    <bs-input-switch asp-for="BooleanProp" />
    <span asp-validation-for="BooleanProp" class="text-danger"></span>
</bs-form-switch>
```

### bs-form-floating <a id="Exemplos-bs-form-floating"></a>

```html
<!-- Texto -->
<bs-form-floating>
    <input asp-for="TextProp" class="form-control" spellcheck="false">
    <bs-form-label asp-for="TextProp"></bs-form-label>
    <span asp-validation-for="TextProp" class="text-danger"></span>
</bs-form-floating>

<!-- TextArea -->
<bs-form-floating>
    <textarea asp-for="TextProp" class="form-control" spellcheck="false"></textarea>
    <bs-form-label asp-for="TextProp"></bs-form-label>
    <span asp-validation-for="TextProp" class="text-danger"></span>
</bs-form-floating>

<!-- Combo Box -->
<bs-form-floating>
    <select asp-for="MasterId" asp-items="SelectListItems" class="form-select form-select-sm">
        <!-- <option value="" selected disabled>Selecione um item</option> -->
    </select>
    <bs-form-label asp-for="MasterId"/>
    <span asp-validation-for="MasterId" class="text-danger"></span>
</bs-form-floating>
<!-- SelectListItems: 
        asp-items="Html.GetEnumSelectList<SomeEnum>()"
        asp-items="@new SelectList(Model.Items, nameof(Item.ValueProp), nameof(Item.DisplayProp))"
-->

```

#### [README](README.md)